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

        let fileName = armaradio.sanitizeFileName(songParts.join(" - ")) + ".mp3";

        if (videoId != "") {
            armaradio.masterPageWait(true);

            armaradio.getFileAsBlob(ajaxPointCall + "/Music/GetAudioFile", {
                ArtistName: artistName,
                SongName: songName,
                VideoId: videoId
            })
                .then(function (blobResponse) {
                    const url = window.URL.createObjectURL(blobResponse);
                    const a = document.createElement("a");
                    a.href = url;
                    a.download = fileName;
                    document.body.appendChild(a);
                    a.click();
                    window.URL.revokeObjectURL(url);

                    armaradio.masterPageWait(false);
                });
        }
    });
}

function loadRadioPlayer(artistName, songName, fromPlaylist, reloadFromCache, fromRandomSongsPlayer) {
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
        }, "Music", "GeRandomtSongsFromPlaylists")
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
    $("#txtRefineParameters_ArtistName").val($.trim(artistName));
    $("#txtRefineParameters_SongName").val($.trim(songName));

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
            $("#lnkRadioOptions").attr({
                "data-videoid": songData.videoId,
                "data-artistname": songData.artist,
                "data-songname": songData.song,
                "data-alternateids": ""
            });
            $("#lblRadioPlayer_SongTitle").html(songData.song);
            $("#lblRadioPlayer_ArtistName").html(songData.artist);

            let newIframe = $("<iframe></iframe");
            newIframe.attr({
                "id": "armaRadioPlayer",
                "class": "iframe-holder pl-0 pt-0",
                "src": "https://www.youtube.com/embed/" + songData.videoId + "?enablejsapi=1",
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
    let lnkControl = $("#lnkRadioOptions");
    let allIds = ($.trim(lnkControl.attr("data-alternateids"))).split(",");

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

function replayWithAlternateId(videoId) {
    let newIframe = $("<iframe></iframe");
    newIframe.attr({
        "id": "armaRadioPlayer",
        "class": "iframe-holder pl-0 pt-0",
        "src": "https://www.youtube.com/embed/" + videoId + "?enablejsapi=1",
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