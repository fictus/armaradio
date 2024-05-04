function loadRadioPlayer(artistName, songName, fromPlaylist) {
    if (artistName != "") {
        armaradio.masterPageWait(true);

        armaradio.masterAJAXPost({
            ArtistName: artistName,
            SongName: songName
        }, "Music", "GetSongsLike")
            .then(function (response) {
                if (response && !response.error) {
                    if (response.length) {
                        $("#dvRadioPlayer_currentlyPlaying")
                            .attr("data-playingid", "")
                            .data("playerSongs", (response || []));

                        $("#lblRadioPlayer_SongTitle").html("");
                        $("#lblRadioPlayer_ArtistName").html("");

                        $("#dvMainPlay_currentlyPlaying").find(".iframe-holder").replaceWith("<div class=\"iframe-holder\"></div>");
                        $("#dvRadioPlayer_currentlyPlaying").find(".iframe-holder").replaceWith("<div class=\"iframe-holder pl-0 pt-0\"></div>");

                        $("#dvRadioPlayerHolder").css("display", "");

                        playNextSong();

                        if (fromPlaylist) {
                            let canvasCtrl;

                            if ($("#offcanvasNonePlaylistOptions").hasClass("show")) {
                                canvasCtrl = $("#offcanvasNonePlaylistOptions");
                            }
                            if ($("#offcanvasSongOptions").hasClass("show")) {
                                canvasCtrl = $("#offcanvasSongOptions");
                            }

                            if (canvasCtrl.length) {
                                canvasCtrl.find("button.btn-close").trigger("click");
                            }
                        }
                    }
                }

                armaradio.masterPageWait(false);
            });
    }
}

function playNextSong() {
    let allSongs = $("#dvRadioPlayer_currentlyPlaying").data("playerSongs") || [];
    let stringId = $.trim($("#dvRadioPlayer_currentlyPlaying").attr("data-playingid"));
    let currentId = 0;

    if (stringId == "") {
        currentId = 1;
        $("#dvRadioPlayer_currentlyPlaying").attr("data-playingid", "1");
    } else {
        currentId = parseInt($("#dvRadioPlayer_currentlyPlaying").attr("data-playingid"));
    }

    let songData = _.where(allSongs, { id: currentId }).pop();

    if (songData) {
        armaradio.masterAJAXPost({
            artistName: songData.artistName,
            songName: songData.songName
        }, "Music", "GetUrlByArtistSongName")
            .then(function (response) {
                if (response) {
                    if (response.hasVideo) {
                        $("#lnkRadioOptions").attr({
                            "data-videoid": response.videoId,
                            "data-artistname": songData.artistName,
                            "data-songname": songData.songName
                        });
                        $("#lblRadioPlayer_SongTitle").html(songData.songName);
                        $("#lblRadioPlayer_ArtistName").html(songData.artistName);

                        let newIframe = $("<iframe></iframe");
                        newIframe.attr({
                            "id": "armaRadioPlayer",
                            "class": "iframe-holder pl-0 pt-0",
                            "src": response.embedUrl,
                            "width": "356", //560
                            "height": "200", //315
                            "frameborder": "0",
                            "allow": "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share",
                            "allowfullscreen": ""
                        });

                        $("#dvRadioPlayer_currentlyPlaying").find(".iframe-holder").remove();
                        $("#dvRadioPlayer_currentlyPlaying").append(newIframe);

                        let player = new YT.Player("armaRadioPlayer", {
                            events: {
                                "onReady": onRadioPlayerReady,
                                "onStateChange": onRadioPlayerStateChange,
                                "onError": onRadioPlayerError
                            }
                        });
                    }
                }

                armaradio.masterPageWait(false);
            });
    } else {
        loadRadioPlayer();
    }
}

function radioPlayerLikeCurrentSong() {

}

function radioPlayerHateCurrentSong() {
    armaradio.masterPageWait(true);

    tallyUpSongId();
    playNextSong();

}

function currentRadioSongOptions() {
    let dataHolder = $("#lnkRadioOptions");
    let artistName = $.trim(dataHolder.attr("data-artistname"));
    let songName = $.trim(dataHolder.attr("data-songname"));
    let videoId = $.trim(dataHolder.attr("data-videoid"));
    let separator = (artistName != "" && songName != "" ? " - " : "");

    $("#btnNonePlaylistOptions_AddToPlaylist").attr({
        "data-artist": artistName,
        "data-song": songName,
        "data-videoid": videoId
    });
    $("#lblNonePlaylistOptionsTitle").html(artistName + separator + songName);

    let bsOffcanvas = new bootstrap.Offcanvas($("#offcanvasNonePlaylistOptions")[0]);
    bsOffcanvas.show();
}

function onRadioPlayerReady(e) {
    e.target.playVideo();
}

function onRadioPlayerStateChange(e) {
    if (e.data == YT.PlayerState.ENDED) {
        tallyUpSongId();
        playNextSong();

        //let currentStatus = $.trim($("#btnMainPlayerToggleRepeat").attr("data-status"));

        //if (currentStatus == "1") {
        //    e.target.playVideo();
        //} else {
            
        //}
    } else {
        console.log(e);
    }
}

function tallyUpSongId() {
    let currentSongId = parseInt($("#dvRadioPlayer_currentlyPlaying").attr("data-playingid")) + 1;
    $("#dvRadioPlayer_currentlyPlaying").attr("data-playingid", currentSongId);
}

function onRadioPlayerError(e) {
    tallyUpSongId();
    playNextSong();
}