mergeInto(LibraryManager.library, {

  Auth: function () {
    auth();
  },

  SetPlayerData: function () {
      myGameInstance.SendMessage('YandexSDK', 'SetName', player.getName());
      myGameInstance.SendMessage('YandexSDK', 'SetPhoto', player.getPhoto("medium"));
  },

  RateGame: function () {
  ysdk.feedback.canReview()
        .then(({ value, reason }) => {
            if (value) {
                ysdk.feedback.requestReview()
                    .then(({ feedbackSent }) => {
                        console.log(feedbackSent);
                        myGameInstance.SendMessage('YandexSDK', 'RateSucces');
                    })
            } else {
                console.log(reason)
            }
        })
  },

  SetToLeaderboard: function (value) {
    ysdk.getLeaderboards()
      .then(lb => {
        lb.setLeaderboardScore('Score', value );
      })
  },

  GetLang: function () {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  ShowAdd: function () {
    ysdk.adv.showFullscreenAdv();
  },

  ShowRewardedVideo: function () {
    ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          console.log('Rewarded!');
        },
        onClose: () => {
          console.log('Video ad closed.');
          myGameInstance.SendMessage('Game Manager', 'Reward');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
    })
  },

});