using System;

namespace HwanLib.MVP.System.BaseMVP
{
    public interface IMultiable
    {
        /// <summary>
        /// UI가 Open되길 원하는 시점에 Invoke 시키기
        /// </summary>
        public event Func<IMultiable, bool> TryOpen;
        public bool CanOpen { get; }
        public void OpenUI();
    }
}