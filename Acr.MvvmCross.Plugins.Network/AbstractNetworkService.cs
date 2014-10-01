using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;


namespace Acr.MvvmCross.Plugins.Network {

    public abstract class AbstractNetworkService : MvxNotifyPropertyChanged, INetworkService
    {

        public AbstractNetworkService()
        {
            this.ShouldAlwaysRaiseInpcOnUserInterfaceThread(true);
        }

        protected void SetStatus(bool connected, bool wifi, bool mobile, bool fireEvent) {
            this.IsConnected = connected;
            this.IsWifi = wifi;
            this.IsMobile = mobile;

            if (fireEvent) { 
                Mvx
                    .Resolve<IMvxMessenger>()
                    .Publish(new NetworkStatusChangedMessage(this));
            }
        }

        #region INetworkService Members

        public abstract Task<bool> IsHostReachable(string host);


        private bool connected;
        public bool IsConnected {
            get { return this.connected; }
            private set {
                if (this.connected == value)
                    return;

                this.connected = value;
                this.RaisePropertyChanged(() => IsConnected);
            }
        }
        
        
        private bool wifi;
        public bool IsWifi {
            get { return this.wifi; }
            private set {
                if (this.wifi == value)
                    return;

                this.wifi = value;
                this.RaisePropertyChanged(() => IsWifi);
            }
        }


        private bool mobile;
        public bool IsMobile {
            get { return this.mobile; }
            private set {
                if (this.mobile == value)
                    return;

                this.mobile = value;
                this.RaisePropertyChanged(() => IsMobile);
            }
        }


        public MvxSubscriptionToken Subscribe(Action<NetworkStatusChangedMessage> action) {
            return Mvx
                .Resolve<IMvxMessenger>()
                .Subscribe<NetworkStatusChangedMessage>(action);
        }

        #endregion
    }
}
