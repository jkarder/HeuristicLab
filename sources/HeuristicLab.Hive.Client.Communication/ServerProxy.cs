﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeuristicLab.Hive.Client.Communication {
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IClientCommunicator")]
  public interface IClientCommunicator {

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IClientCommunicator/Login", ReplyAction = "http://tempuri.org/IClientCommunicator/LoginResponse")]
    HeuristicLab.Hive.Contracts.Response Login(HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo clientInfo);

    [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IClientCommunicator/Login", ReplyAction = "http://tempuri.org/IClientCommunicator/LoginResponse")]
    System.IAsyncResult BeginLogin(HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo clientInfo, System.AsyncCallback callback, object asyncState);

    HeuristicLab.Hive.Contracts.Response EndLogin(System.IAsyncResult result);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IClientCommunicator/SendHeartBeat", ReplyAction = "http://tempuri.org/IClientCommunicator/SendHeartBeatResponse")]
    HeuristicLab.Hive.Contracts.ResponseHB SendHeartBeat(HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData hbData);

    [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IClientCommunicator/SendHeartBeat", ReplyAction = "http://tempuri.org/IClientCommunicator/SendHeartBeatResponse")]
    System.IAsyncResult BeginSendHeartBeat(HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData hbData, System.AsyncCallback callback, object asyncState);

    HeuristicLab.Hive.Contracts.ResponseHB EndSendHeartBeat(System.IAsyncResult result);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IClientCommunicator/PullJob", ReplyAction = "http://tempuri.org/IClientCommunicator/PullJobResponse")]
    HeuristicLab.Hive.Contracts.ResponseJob PullJob(System.Guid clientId);

    [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IClientCommunicator/PullJob", ReplyAction = "http://tempuri.org/IClientCommunicator/PullJobResponse")]
    System.IAsyncResult BeginPullJob(System.Guid clientId, System.AsyncCallback callback, object asyncState);

    HeuristicLab.Hive.Contracts.ResponseJob EndPullJob(System.IAsyncResult result);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IClientCommunicator/SendJobResult", ReplyAction = "http://tempuri.org/IClientCommunicator/SendJobResultResponse")]
    HeuristicLab.Hive.Contracts.Response SendJobResult(HeuristicLab.Hive.Contracts.BusinessObjects.JobResult Result, bool finished);

    [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IClientCommunicator/SendJobResult", ReplyAction = "http://tempuri.org/IClientCommunicator/SendJobResultResponse")]
    System.IAsyncResult BeginSendJobResult(HeuristicLab.Hive.Contracts.BusinessObjects.JobResult Result, bool finished, System.AsyncCallback callback, object asyncState);

    HeuristicLab.Hive.Contracts.Response EndSendJobResult(System.IAsyncResult result);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IClientCommunicator/Logout", ReplyAction = "http://tempuri.org/IClientCommunicator/LogoutResponse")]
    HeuristicLab.Hive.Contracts.Response Logout(System.Guid clientId);

    [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IClientCommunicator/Logout", ReplyAction = "http://tempuri.org/IClientCommunicator/LogoutResponse")]
    System.IAsyncResult BeginLogout(System.Guid clientId, System.AsyncCallback callback, object asyncState);

    HeuristicLab.Hive.Contracts.Response EndLogout(System.IAsyncResult result);
  }

  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  public interface IClientCommunicatorChannel : IClientCommunicator, System.ServiceModel.IClientChannel {
  }

  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  public partial class LoginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {

    private object[] results;

    public LoginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
      base(exception, cancelled, userState) {
      this.results = results;
    }

    public HeuristicLab.Hive.Contracts.Response Result {
      get {
        base.RaiseExceptionIfNecessary();
        return ((HeuristicLab.Hive.Contracts.Response)(this.results[0]));
      }
    }
  }

  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  public partial class SendHeartBeatCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {

    private object[] results;

    public SendHeartBeatCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
      base(exception, cancelled, userState) {
      this.results = results;
    }

    public HeuristicLab.Hive.Contracts.ResponseHB Result {
      get {
        base.RaiseExceptionIfNecessary();
        return ((HeuristicLab.Hive.Contracts.ResponseHB)(this.results[0]));
      }
    }
  }

  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  public partial class PullJobCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {

    private object[] results;

    public PullJobCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
      base(exception, cancelled, userState) {
      this.results = results;
    }

    public HeuristicLab.Hive.Contracts.ResponseJob Result {
      get {
        base.RaiseExceptionIfNecessary();
        return ((HeuristicLab.Hive.Contracts.ResponseJob)(this.results[0]));
      }
    }
  }

  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  public partial class SendJobResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {

    private object[] results;

    public SendJobResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
      base(exception, cancelled, userState) {
      this.results = results;
    }

    public HeuristicLab.Hive.Contracts.Response Result {
      get {
        base.RaiseExceptionIfNecessary();
        return ((HeuristicLab.Hive.Contracts.Response)(this.results[0]));
      }
    }
  }

  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  public partial class LogoutCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {

    private object[] results;

    public LogoutCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
      base(exception, cancelled, userState) {
      this.results = results;
    }

    public HeuristicLab.Hive.Contracts.Response Result {
      get {
        base.RaiseExceptionIfNecessary();
        return ((HeuristicLab.Hive.Contracts.Response)(this.results[0]));
      }
    }
  }

  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
  public partial class ClientCommunicatorClient : System.ServiceModel.ClientBase<IClientCommunicator>, IClientCommunicator {

    private BeginOperationDelegate onBeginLoginDelegate;

    private EndOperationDelegate onEndLoginDelegate;

    private System.Threading.SendOrPostCallback onLoginCompletedDelegate;

    private BeginOperationDelegate onBeginSendHeartBeatDelegate;

    private EndOperationDelegate onEndSendHeartBeatDelegate;

    private System.Threading.SendOrPostCallback onSendHeartBeatCompletedDelegate;

    private BeginOperationDelegate onBeginPullJobDelegate;

    private EndOperationDelegate onEndPullJobDelegate;

    private System.Threading.SendOrPostCallback onPullJobCompletedDelegate;

    private BeginOperationDelegate onBeginSendJobResultDelegate;

    private EndOperationDelegate onEndSendJobResultDelegate;

    private System.Threading.SendOrPostCallback onSendJobResultCompletedDelegate;

    private BeginOperationDelegate onBeginLogoutDelegate;

    private EndOperationDelegate onEndLogoutDelegate;

    private System.Threading.SendOrPostCallback onLogoutCompletedDelegate;

    public ClientCommunicatorClient() {
    }

    public ClientCommunicatorClient(string endpointConfigurationName) :
      base(endpointConfigurationName) {
    }

    public ClientCommunicatorClient(string endpointConfigurationName, string remoteAddress) :
      base(endpointConfigurationName, remoteAddress) {
    }

    public ClientCommunicatorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
      base(endpointConfigurationName, remoteAddress) {
    }

    public ClientCommunicatorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
      base(binding, remoteAddress) {
    }

    public event System.EventHandler<LoginCompletedEventArgs> LoginCompleted;

    public event System.EventHandler<SendHeartBeatCompletedEventArgs> SendHeartBeatCompleted;

    public event System.EventHandler<PullJobCompletedEventArgs> PullJobCompleted;

    public event System.EventHandler<SendJobResultCompletedEventArgs> SendJobResultCompleted;

    public event System.EventHandler<LogoutCompletedEventArgs> LogoutCompleted;

    public HeuristicLab.Hive.Contracts.Response Login(HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo clientInfo) {
      return base.Channel.Login(clientInfo);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginLogin(HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo clientInfo, System.AsyncCallback callback, object asyncState) {
      return base.Channel.BeginLogin(clientInfo, callback, asyncState);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public HeuristicLab.Hive.Contracts.Response EndLogin(System.IAsyncResult result) {
      return base.Channel.EndLogin(result);
    }

    private System.IAsyncResult OnBeginLogin(object[] inValues, System.AsyncCallback callback, object asyncState) {
      HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo clientInfo = ((HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo)(inValues[0]));
      return this.BeginLogin(clientInfo, callback, asyncState);
    }

    private object[] OnEndLogin(System.IAsyncResult result) {
      HeuristicLab.Hive.Contracts.Response retVal = this.EndLogin(result);
      return new object[] {
                retVal};
    }

    private void OnLoginCompleted(object state) {
      if ((this.LoginCompleted != null)) {
        InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
        this.LoginCompleted(this, new LoginCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
      }
    }

    public void LoginAsync(HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo clientInfo) {
      this.LoginAsync(clientInfo, null);
    }

    public void LoginAsync(HeuristicLab.Hive.Contracts.BusinessObjects.ClientInfo clientInfo, object userState) {
      if ((this.onBeginLoginDelegate == null)) {
        this.onBeginLoginDelegate = new BeginOperationDelegate(this.OnBeginLogin);
      }
      if ((this.onEndLoginDelegate == null)) {
        this.onEndLoginDelegate = new EndOperationDelegate(this.OnEndLogin);
      }
      if ((this.onLoginCompletedDelegate == null)) {
        this.onLoginCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLoginCompleted);
      }
      base.InvokeAsync(this.onBeginLoginDelegate, new object[] {
                    clientInfo}, this.onEndLoginDelegate, this.onLoginCompletedDelegate, userState);
    }

    public HeuristicLab.Hive.Contracts.ResponseHB SendHeartBeat(HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData hbData) {
      return base.Channel.SendHeartBeat(hbData);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginSendHeartBeat(HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData hbData, System.AsyncCallback callback, object asyncState) {
      return base.Channel.BeginSendHeartBeat(hbData, callback, asyncState);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public HeuristicLab.Hive.Contracts.ResponseHB EndSendHeartBeat(System.IAsyncResult result) {
      return base.Channel.EndSendHeartBeat(result);
    }

    private System.IAsyncResult OnBeginSendHeartBeat(object[] inValues, System.AsyncCallback callback, object asyncState) {
      HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData hbData = ((HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData)(inValues[0]));
      return this.BeginSendHeartBeat(hbData, callback, asyncState);
    }

    private object[] OnEndSendHeartBeat(System.IAsyncResult result) {
      HeuristicLab.Hive.Contracts.ResponseHB retVal = this.EndSendHeartBeat(result);
      return new object[] {
                retVal};
    }

    private void OnSendHeartBeatCompleted(object state) {
      if ((this.SendHeartBeatCompleted != null)) {
        InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
        this.SendHeartBeatCompleted(this, new SendHeartBeatCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
      }
    }

    public void SendHeartBeatAsync(HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData hbData) {
      this.SendHeartBeatAsync(hbData, null);
    }

    public void SendHeartBeatAsync(HeuristicLab.Hive.Contracts.BusinessObjects.HeartBeatData hbData, object userState) {
      if ((this.onBeginSendHeartBeatDelegate == null)) {
        this.onBeginSendHeartBeatDelegate = new BeginOperationDelegate(this.OnBeginSendHeartBeat);
      }
      if ((this.onEndSendHeartBeatDelegate == null)) {
        this.onEndSendHeartBeatDelegate = new EndOperationDelegate(this.OnEndSendHeartBeat);
      }
      if ((this.onSendHeartBeatCompletedDelegate == null)) {
        this.onSendHeartBeatCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSendHeartBeatCompleted);
      }
      base.InvokeAsync(this.onBeginSendHeartBeatDelegate, new object[] {
                    hbData}, this.onEndSendHeartBeatDelegate, this.onSendHeartBeatCompletedDelegate, userState);
    }

    public HeuristicLab.Hive.Contracts.ResponseJob PullJob(System.Guid clientId) {
      return base.Channel.PullJob(clientId);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginPullJob(System.Guid clientId, System.AsyncCallback callback, object asyncState) {
      return base.Channel.BeginPullJob(clientId, callback, asyncState);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public HeuristicLab.Hive.Contracts.ResponseJob EndPullJob(System.IAsyncResult result) {
      return base.Channel.EndPullJob(result);
    }

    private System.IAsyncResult OnBeginPullJob(object[] inValues, System.AsyncCallback callback, object asyncState) {
      System.Guid clientId = ((System.Guid)(inValues[0]));
      return this.BeginPullJob(clientId, callback, asyncState);
    }

    private object[] OnEndPullJob(System.IAsyncResult result) {
      HeuristicLab.Hive.Contracts.ResponseJob retVal = this.EndPullJob(result);
      return new object[] {
                retVal};
    }

    private void OnPullJobCompleted(object state) {
      if ((this.PullJobCompleted != null)) {
        InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
        this.PullJobCompleted(this, new PullJobCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
      }
    }

    public void PullJobAsync(System.Guid clientId) {
      this.PullJobAsync(clientId, null);
    }

    public void PullJobAsync(System.Guid clientId, object userState) {
      if ((this.onBeginPullJobDelegate == null)) {
        this.onBeginPullJobDelegate = new BeginOperationDelegate(this.OnBeginPullJob);
      }
      if ((this.onEndPullJobDelegate == null)) {
        this.onEndPullJobDelegate = new EndOperationDelegate(this.OnEndPullJob);
      }
      if ((this.onPullJobCompletedDelegate == null)) {
        this.onPullJobCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnPullJobCompleted);
      }
      base.InvokeAsync(this.onBeginPullJobDelegate, new object[] {
                    clientId}, this.onEndPullJobDelegate, this.onPullJobCompletedDelegate, userState);
    }

    public HeuristicLab.Hive.Contracts.Response SendJobResult(HeuristicLab.Hive.Contracts.BusinessObjects.JobResult Result, bool finished) {
      return base.Channel.SendJobResult(Result, finished);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginSendJobResult(HeuristicLab.Hive.Contracts.BusinessObjects.JobResult Result, bool finished, System.AsyncCallback callback, object asyncState) {
      return base.Channel.BeginSendJobResult(Result, finished, callback, asyncState);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public HeuristicLab.Hive.Contracts.Response EndSendJobResult(System.IAsyncResult result) {
      return base.Channel.EndSendJobResult(result);
    }

    private System.IAsyncResult OnBeginSendJobResult(object[] inValues, System.AsyncCallback callback, object asyncState) {
      HeuristicLab.Hive.Contracts.BusinessObjects.JobResult Result = ((HeuristicLab.Hive.Contracts.BusinessObjects.JobResult)(inValues[0]));
      bool finished = ((bool)(inValues[1]));
      return this.BeginSendJobResult(Result, finished, callback, asyncState);
    }

    private object[] OnEndSendJobResult(System.IAsyncResult result) {
      HeuristicLab.Hive.Contracts.Response retVal = this.EndSendJobResult(result);
      return new object[] {
                retVal};
    }

    private void OnSendJobResultCompleted(object state) {
      if ((this.SendJobResultCompleted != null)) {
        InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
        this.SendJobResultCompleted(this, new SendJobResultCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
      }
    }

    public void SendJobResultAsync(HeuristicLab.Hive.Contracts.BusinessObjects.JobResult Result, bool finished) {
      this.SendJobResultAsync(Result, finished, null);
    }

    public void SendJobResultAsync(HeuristicLab.Hive.Contracts.BusinessObjects.JobResult Result, bool finished, object userState) {
      if ((this.onBeginSendJobResultDelegate == null)) {
        this.onBeginSendJobResultDelegate = new BeginOperationDelegate(this.OnBeginSendJobResult);
      }
      if ((this.onEndSendJobResultDelegate == null)) {
        this.onEndSendJobResultDelegate = new EndOperationDelegate(this.OnEndSendJobResult);
      }
      if ((this.onSendJobResultCompletedDelegate == null)) {
        this.onSendJobResultCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSendJobResultCompleted);
      }
      base.InvokeAsync(this.onBeginSendJobResultDelegate, new object[] {
                    Result,
                    finished}, this.onEndSendJobResultDelegate, this.onSendJobResultCompletedDelegate, userState);
    }

    public HeuristicLab.Hive.Contracts.Response Logout(System.Guid clientId) {
      return base.Channel.Logout(clientId);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public System.IAsyncResult BeginLogout(System.Guid clientId, System.AsyncCallback callback, object asyncState) {
      return base.Channel.BeginLogout(clientId, callback, asyncState);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public HeuristicLab.Hive.Contracts.Response EndLogout(System.IAsyncResult result) {
      return base.Channel.EndLogout(result);
    }

    private System.IAsyncResult OnBeginLogout(object[] inValues, System.AsyncCallback callback, object asyncState) {
      System.Guid clientId = ((System.Guid)(inValues[0]));
      return this.BeginLogout(clientId, callback, asyncState);
    }

    private object[] OnEndLogout(System.IAsyncResult result) {
      HeuristicLab.Hive.Contracts.Response retVal = this.EndLogout(result);
      return new object[] {
                retVal};
    }

    private void OnLogoutCompleted(object state) {
      if ((this.LogoutCompleted != null)) {
        InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
        this.LogoutCompleted(this, new LogoutCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
      }
    }

    public void LogoutAsync(System.Guid clientId) {
      this.LogoutAsync(clientId, null);
    }

    public void LogoutAsync(System.Guid clientId, object userState) {
      if ((this.onBeginLogoutDelegate == null)) {
        this.onBeginLogoutDelegate = new BeginOperationDelegate(this.OnBeginLogout);
      }
      if ((this.onEndLogoutDelegate == null)) {
        this.onEndLogoutDelegate = new EndOperationDelegate(this.OnEndLogout);
      }
      if ((this.onLogoutCompletedDelegate == null)) {
        this.onLogoutCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLogoutCompleted);
      }
      base.InvokeAsync(this.onBeginLogoutDelegate, new object[] {
                    clientId}, this.onEndLogoutDelegate, this.onLogoutCompletedDelegate, userState);
    }
  }
}

