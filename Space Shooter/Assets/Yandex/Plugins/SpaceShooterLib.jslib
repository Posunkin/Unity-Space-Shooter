mergeInto(LibraryManager.library, {

  SetPlayerData: function () {
    myGameInstance.SendMessage('YandexSDK', 'SetName', player.getName());
    myGameInstance.SendMessage('YandexSDK', 'SetPhoto', player.getPhoto("medium"));
  },


});