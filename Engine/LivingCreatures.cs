using System.ComponentModel;

namespace Engine
{
    public class LivingCreature : INotifyPropertyChanged
    {
        private int _currentHitPoints;
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged("CurrentHitPoints");
            }
        }
        public int MaximumHitPoints { get; set; }
        protected int _experiencePoints;
        public int ExperiencePoints 
        {
            get { return _experiencePoints; }
            set { _experiencePoints = value; }
        }

        public LivingCreature(int currentHitPoints, int maximumHitPoints, int experiencePoints)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
            ExperiencePoints = experiencePoints;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
