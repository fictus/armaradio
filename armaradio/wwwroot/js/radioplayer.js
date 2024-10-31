var radioPlayerMain;

function radioplayer_attachEvents() {
    $(document).on("keydown", function (e) {
        if (e.ctrlKey) {
            $(document).data("allowDownload", true);
        }
    });

    $(document).on("keyup", function () {
        $(document).data("allowDownload", false);
    });

    $(".btn-options-download").on("click", function () {
        let btn = $(this);
        let videoId = $.trim(btn.attr("data-videoid"));
        let artistName = $.trim(btn.attr("data-artist"));
        let songName = $.trim(btn.attr("data-song"));
        let songParts = [];

        if (artistName != "") {
            songParts.push(artistName);
        }
        if (songName != "") {
            songParts.push(songName);
        }

        let fileName = armaradio.sanitizeFileName(songParts.join(" - "));

        if (videoId != "") {
            armaradio.masterPageWait(true);

            let fileExtension = "m4a";
            let mimeType = "audio/mp4";

            armaradio.getFileAsBlob(ajaxPointCall + "/Music/GetAudioFile", {
                ArtistName: artistName,
                SongName: songName,
                VideoId: videoId
            }, mimeType)
                .then(function (blobResponse) {
                    let url = window.URL.createObjectURL(blobResponse);
                    let a = $("<a></a>");
                    a.attr({
                        "id": "lnkTempDownloadFile",
                        "href": url,
                        "download": (fileName + "." + fileExtension)
                    });

                    $("body").append(a);

                    a[0].click();
                    window.URL.revokeObjectURL(url);

                    setTimeout(function () {
                        $("#lnkTempDownloadFile").remove();
                    }, 1000);

                    armaradio.masterPageWait(false);
                });
        }
    });
}

function loadRadioPlayer(artistName, songName, fromPlaylist, reloadFromCache, fromRandomSongsPlayer) {
    if (localHomePlayer) {
        try {
            localHomePlayer.dispose();
            localHomePlayer = null;
        } catch (ex) {

        }
    }

    if (radioPlayerMain) {
        try {
            radioPlayerMain.dispose();
            radioPlayerMain = null;
        } catch (ex) {

        }
    }

    if (!fromRandomSongsPlayer) {
        if (artistName != "") {
            armaradio.masterPageWait(true);

            if (!reloadFromCache) {
                $("#lnkRadioOptions").attr({
                    "data-artist": artistName,
                    "data-song": songName
                });
            } else {
                let cachedValues = $("#lnkRadioOptions");
                artistName = artistName || cachedValues.attr("data-artist");
                songName = songName || cachedValues.attr("data-song");
            }

            armaradio.masterAJAXPost({
                ArtistName: artistName,
                SongName: songName
            }, "Music", "GetSongsLike")
                .then(function (response) {
                    if (response && !response.error) {
                        if (response.length) {
                            $("#dvRadioPlayer_currentlyPlaying")
                                .attr({
                                    "data-israndom": "0",
                                    "data-playingid": ""
                                })
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
                        } else {
                            showNoRadioFound(artistName, songName);
                        }
                    } else {
                        showNoRadioFound(artistName, songName);
                    }

                    armaradio.masterPageWait(false);
                });
        }
    } else {
        // start playing from random songs in user's playlists
        armaradio.masterPageWait(true);

        armaradio.masterAJAXPost({
            ArtistName: artistName,
            SongName: songName
        }, "Music", "GeRandomSongsFromPlaylists")
            .then(function (response) {
                if (response && !response.error) {
                    if (response.length) {
                        let allSongs = [];

                        for (let i = 0; i < response.length; i++) {
                            let currentSong = response[i];
                            currentSong["id"] = i + 1;

                            allSongs.push(currentSong);
                        }

                        $("#dvRadioPlayer_currentlyPlaying")
                            .attr({
                                "data-israndom": "1",
                                "data-playingid": ""
                            })
                            .data("playerSongs", (allSongs || []));

                        $("#lblRadioPlayer_SongTitle").html("");
                        $("#lblRadioPlayer_ArtistName").html("");

                        $("#dvMainPlay_currentlyPlaying").find(".iframe-holder").replaceWith("<div class=\"iframe-holder\"></div>");
                        $("#dvRadioPlayer_currentlyPlaying").find(".iframe-holder").replaceWith("<div class=\"iframe-holder pl-0 pt-0\"></div>");

                        $("#dvRadioPlayerHolder").css("display", "");

                        if ($("#dvPopupLoadPlaylists").is(":visible")) {
                            $("#dvPopupLoadPlaylists").modal("hide");
                        }

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
                    } else {
                        armaradio.warningMsg({
                            msg: "You must have songs in your Playlists",
                            captionMsg: "Empty Playlists",
                            typeLayout: "red"
                        });
                    }
                } else {
                    armaradio.warningMsg({
                        msg: response.errorMsg,
                        captionMsg: "Error",
                        typeLayout: "red"
                    });
                }

                armaradio.masterPageWait(false);
            });
    }
}

function showNoRadioFound(artistName, songName) {
    $("#lblRefineRadioParameters_Title").html("Refine Radio Parameters");
    $("#lblRefineRadioParameters_Message").html("Unable to create a radio session from your selection. Please refine these parameters and try again:");

    $("#txtRefineParameters_ArtistName").val($.trim(artistName));
    $("#txtRefineParameters_SongName").val($.trim(songName));

    $("#dvPopupRefineRadioParameters").modal("show");
}

function showRadioSearchBox() {
    $("#lblRefineRadioParameters_Title").html("Start Radio Session");
    $("#lblRefineRadioParameters_Message").html("Enter Artist/Song to start radio session:");

    $("#txtRefineParameters_ArtistName").val("");
    $("#txtRefineParameters_SongName").val("");

    $("#dvPopupRefineRadioParameters").modal("show");
}

function radioPlayerRefineParametersStartRadio() {
    let artistName = $.trim($("#txtRefineParameters_ArtistName").val());
    let songName = $.trim($("#txtRefineParameters_SongName").val());

    if (artistName == "" && songName == "") {
        armaradio.warningMsg({
            msg: "You must specify at least one parameter",
            captionMsg: "Error",
            typeLayout: "red"
        });
    } else {
        loadRadioPlayer(artistName, songName, true);

        $("#dvPopupRefineRadioParameters").modal("hide");
    }
}

function playNextSong() {
    let fromRandomSongs = $.trim($("#dvRadioPlayer_currentlyPlaying").attr("data-israndom")) == "1";
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
        if (!fromRandomSongs) {
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
                                "data-songname": songData.songName,
                                "data-alternateids": (response.alternateIds || []).join(",")
                            });
                            $("#lblRadioPlayer_SongTitle").html(songData.songName);
                            $("#lblRadioPlayer_ArtistName").html(songData.artistName);

                            $("a.lnk-attribution-notice").attr("data-artistname", songData.artistName);
                            $("a.lnk-attribution-notice").attr("data-songname", songData.songName);
                            $("a.lnk-attribution-notice").attr("data-url", "https://www.youtube.com/watch?v=" + response.videoId);

                            initializeRadioPlayer(response.videoId);
                        }
                    }

                    armaradio.masterPageWait(false);
                });
        } else {
            $("#lnkRadioOptions").attr({
                "data-videoid": songData.videoId,
                "data-artistname": songData.artist,
                "data-songname": songData.song,
                "data-alternateids": ""
            });
            $("#lblRadioPlayer_SongTitle").html(songData.song);
            $("#lblRadioPlayer_ArtistName").html(songData.artist);

            $("a.lnk-attribution-notice").attr("data-artistname", songData.artist);
            $("a.lnk-attribution-notice").attr("data-songname", songData.song);
            $("a.lnk-attribution-notice").attr("data-url", "https://www.youtube.com/watch?v=" + songData.videoId);

            initializeRadioPlayer(songData.videoId);
        }
    } else {
        loadRadioPlayer(null, null, false, true, fromRandomSongs);
    }
}

function radioPlayerLikeCurrentSong() {

}

function radioPlayerHateCurrentSong() {
    let fromRandomSongs = $.trim($("#dvRadioPlayer_currentlyPlaying").attr("data-israndom")) == "1";

    if (!fromRandomSongs) {
        armaradio.masterPageWait(true);
    }

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
    $(".btn-options-download").attr({
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

function onRadioPlayerStateChange() {
    tallyUpSongId();
    playNextSong();
}

//function onRadioPlayerStateChange(e) {
//    if (e.data == YT.PlayerState.ENDED) {
//        tallyUpSongId();
//        playNextSong();

//        //let currentStatus = $.trim($("#btnMainPlayerToggleRepeat").attr("data-status"));

//        //if (currentStatus == "1") {
//        //    e.target.playVideo();
//        //} else {
            
//        //}
//    } else {
//        console.log(e);
//    }
//}

function tallyUpSongId() {
    let currentSongId = parseInt($("#dvRadioPlayer_currentlyPlaying").attr("data-playingid")) + 1;
    $("#dvRadioPlayer_currentlyPlaying").attr("data-playingid", currentSongId);
}

function onRadioPlayerError(e) {
    let lnkControl = $("#lnkRadioOptions");
    let allIds = ($.trim(lnkControl.attr("data-alternateids")) == "" ? [] : ($.trim(lnkControl.attr("data-alternateids"))).split(","));

    if (allIds.length) {
        let videoId = allIds[0];
        allIds.shift();

        lnkControl.attr({
            "data-videoid": videoId,
            "data-alternateids": (allIds || []).join(",")
        });

        replayWithAlternateId(videoId);
    } else {
        tallyUpSongId();
        playNextSong();
    }
}

function initializeRadioPlayer(videoId, dispose) {
    $("a.lnk-attribution-notice").css("display", "none");

    if (!radioPlayerMain || dispose) {
        let newIframe = $("<video></video");
        newIframe.attr({
            "id": "armaMainPlayer",
            "class": "iframe-holder video-js vjs-default-skin"
        });

        if (radioPlayerMain) {
            try {
                radioPlayerMain.dispose();
                radioPlayerMain = null;
            } catch (ex) {

            }
        }

        $("#dvRadioPlayer_currentlyPlaying").find(".iframe-holder").remove();
        $("#dvRadioPlayer_currentlyPlaying").append(newIframe);

        radioPlayerMain = videojs("armaMainPlayer", {
            width: 356,
            height: 200,
            muted: true,
            autoplay: false,
            controls: true,
            poster: "https://random-image-pepebigotes.vercel.app/api/random-image?g=" + generateGUID(),
            sources: [{
                src: (ajaxPointCall + "/Music/FetchAudioFile?VideoId=" + videoId),
                type: "audio/mp4"
            }]
        });
        radioPlayerMain.ready(function () {
            restoreVolume(this);

            this.soundWave({
                waveColor: soundWaveColor,
                waveWidth: 356,
                waveHeight: 200
            });

            // Attempt to play only after user interaction
            //const playButton = this.el().querySelector(".vjs-big-play-button") || this.el();
            //playButton.addEventListener("click", function () {
            //    radioPlayerMain.play().catch(error => {
            //        console.error("Playback failed:", error);
            //    });
            //});

            //if (!isIOSDevice) {
                $.when(radioPlayerMain.play())
                    .then(function () {
                        setTimeout(function () {
                            $("a.lnk-attribution-notice").css("display", "");

                            radioPlayerMain.muted(false);

                            prebufferNextRadioSong();
                        }, 100);
                    });
            //} else {
            //    setTimeout(function () {
            //        radioPlayerMain.muted(false);
            //    }, 100);
            //}
        });
        //radioPlayerMain.soundWave({
        //    waveColor: soundWaveColor,
        //    waveWidth: 356,
        //    waveHeight: 200
        //});

        radioPlayerMain.on("error", function (e) {
            console.log(e);

            $("a.lnk-attribution-notice").css("display", "none");
            onRadioPlayerError();
        });
        //radioPlayerMain.on("ready", function () {
        //    restoreVolume(radioPlayerMain);
        //    radioPlayerMain.play();
        //});
        radioPlayerMain.on("ended", function () {
            $("a.lnk-attribution-notice").css("display", "none");

            onRadioPlayerStateChange();
        });
        radioPlayerMain.on("previous", function () {
            radioPlayerMain.pause();
            radioPlayerMain.currentTime(0);
            radioPlayerMain.play();
        });
        radioPlayerMain.on("next", function () {
            tallyUpSongId();
            playNextSong();
        });
        radioPlayerMain.on("volumechange", function () {
            saveVolume(radioPlayerMain);
        });
    } else {
        radioPlayerMain.pause();

        let newPoster = "https://random-image-pepebigotes.vercel.app/api/random-image?g=" + generateGUID();
        let newSource = ajaxPointCall + "/Music/FetchAudioFile?VideoId=" + videoId;

        radioPlayerMain.poster(newPoster);
        radioPlayerMain.src({
            type: "audio/mp4",
            src: newSource
        });

        $.when(radioPlayerMain.load())
            .then(function () {
                $.when(radioPlayerMain.play())
                    .then(function () {
                        setTimeout(function () {
                            $("a.lnk-attribution-notice").css("display", "");

                            radioPlayerMain.muted(false);

                            prebufferNextRadioSong();
                        }, 100);
                    });
            });

        //radioPlayerMain.play();
    }
}

function prebufferNextRadioSong() {
    let allSongs = $("#dvRadioPlayer_currentlyPlaying").data("playerSongs") || [];
    let stringId = $.trim($("#dvRadioPlayer_currentlyPlaying").attr("data-playingid"));
    let currentId = -1;

    if (stringId != "") {
        currentId = parseInt($("#dvRadioPlayer_currentlyPlaying").attr("data-playingid"));
        currentId = currentId + 1;
    }

    let songData = _.where(allSongs, { id: currentId }).pop();

    if (songData) {
        let videoId = $.trim(songData.videoId);

        if (videoId != "") {
            fetch(ajaxPointCall + "/Music/FetchAudioFile?VideoId=" + videoId, {
                method: "GET",
                headers: {
                    "Range": "bytes=0-1"
                }
            });
        }
    }
}

function replayWithAlternateId(videoId) {
    $("a.lnk-attribution-notice").attr("data-url", "https://www.youtube.com/watch?v=" + videoId);

    initializeRadioPlayer(videoId);
}