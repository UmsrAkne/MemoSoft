using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MemoSoft
{
    public abstract class INotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null) {
            var h = this.PropertyChanged;
            if (h != null) h(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T target, T value,[CallerMemberName]string propertyName = null) {
            if (Equals(target, value)) return false; // プロパティをセットした際、比較して等しければ、セットせずリターンする。
            target = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
