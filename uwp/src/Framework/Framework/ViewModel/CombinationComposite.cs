using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    ///     组合 Composite 和 Message
    /// </summary>
    public class CombinationComposite : Composite, IMessage, ICombinationComposite
    {
        public CombinationComposite(ViewModelBase source)
        {
            ((IMessage) this).Source = source;
        }

        public CombinationComposite(Action<ViewModelBase, object> run, ViewModelBase source)
        {
            _run = run;
            ((IMessage) this).Source = source;
        }

        public override void Run(ViewModelBase source, IMessage message)
        {
            _run.Invoke(source, message);
        }

        ViewModelBase IMessage.Source { get; set; }

        /// <inheritdoc />
        public IPredicateViewModel Goal { set; get; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            if (Goal == null)
            {
                return true;
            }

            return Goal.Predicate(viewModel);
        }

        protected Action<ViewModelBase, object> _run;
    }

    public class CombinationComposite<T> : Composite, IMessage, ICombinationComposite
        where T : IViewModel
    {
        public CombinationComposite(ViewModelBase source)
        {
            ((IMessage) this).Source = source;
            Goal = new PredicateInheritViewModel(typeof(T));
        }

        public CombinationComposite(Action<T> run, ViewModelBase source) : this(source)
        {
            _run = run;
        }

        public override void Run(IViewModel source, IMessage message)
        {
            if (source is T)
            {
                _run.Invoke((T) source);
            }
        }

        ViewModelBase IMessage.Source { get; set; }

        /// <inheritdoc />
        public IPredicateViewModel Goal { set; get; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            if (Goal == null)
            {
                return viewModel is T;
            }

            return Goal.Predicate(viewModel);
        }

        protected Action<T> _run;
    }

    /// <summary>
    ///     组合 Composite 和 Message
    /// </summary>
    public class CombinationComposite<T, U> : Composite, IMessage, ICombinationComposite
        where U : IMessage where T : IViewModel
    {
        public CombinationComposite(ViewModelBase source)
        {
            ((IMessage) this).Source = source;
            Goal = new PredicateInheritViewModel(typeof(T));
        }

        public CombinationComposite(Action<T, U> run, ViewModelBase source) : this(source)
        {
            _run = run;
        }

        public override void Run(IViewModel source, IMessage message)
        {
            if (source is T && message is U)
            {
                _run.Invoke((T) source, (U) message);
            }
        }

        ViewModelBase IMessage.Source { get; set; }

        /// <inheritdoc />
        public IPredicateViewModel Goal { set; get; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            if (Goal == null)
            {
                return viewModel is T;
            }

            return Goal.Predicate(viewModel);
        }

        protected Action<T, U> _run;
    }
}