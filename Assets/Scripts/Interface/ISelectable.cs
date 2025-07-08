namespace AnalyticalApproach.OrbAscent
{
    public interface ISelectable
    {
        public abstract void OnSelected(); 
        public abstract void OnDeselected(); 

        public virtual bool CanBeSelected() { return true;  }
        public virtual bool CanBeDeselected() { return true;  }
    }
}
