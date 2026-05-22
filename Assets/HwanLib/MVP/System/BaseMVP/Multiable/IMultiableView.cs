namespace HwanLib.MVP.System.BaseMVP.Multiable
{
    public interface IMultiableView
    {
        /// <summary>
        /// UI를 킬 수 있는지 확인하는 bool
        /// </summary>
        public bool CanOpen { get; }
    }
}