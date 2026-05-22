using System;

namespace HwanLib.MVP.System.BaseMVP.Multiable
{
    public interface IMultiablePresenter
    {
        /// <summary>
        /// UI가 Open되길 원하는 시점에 Invoke 시키기
        /// </summary>
        public event Func<IMultiablePresenter, bool> TryOpen;
        public IMultiableView MultiableView { get; }
        /// <summary>
        /// UI를 키는 로직
        /// </summary>
        public void OpenUI();
    }
}