namespace MVP.System
{
    public abstract class BaseForm
    {
        public string TargetMethodName { get; private set; }
        
        private readonly InteractiveObject _onInteractiveObject;

        private BaseForm(InteractiveObject onInteractiveObject)
        {
            _onInteractiveObject = onInteractiveObject;
        }

        protected T OnInteractive<T>(T value) where T : BaseUIData
        {
            return _onInteractiveObject?.Invoke(TargetMethodName, value) as T;
        }
    }
}