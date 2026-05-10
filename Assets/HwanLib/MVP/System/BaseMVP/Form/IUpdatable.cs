namespace HwanLib.MVP.System.BaseMVP.Form
{
    public interface IUpdatable
    {
        public event UpdateForm OnFormUpdate;
        
        public void UpdateForm();
    }
}