$(document).ready(function () {
    mainload_attacheEvents();
});

function mainload_attacheEvents() {
    $("#dvPopupPastePlaylist").modal();

    armaradio.masterAJAXGet({}, "Music", "GetCurrentTop100")
        .then(function (response) {
            attachListToTable(response, true);
        });

    $("#btnMain_PlayTop100").on("click", function () {
        armaradio.masterPageWait(true);

        armaradio.masterAJAXGet({}, "Music", "GetCurrentTop100")
            .then(function (response) {
                attachListToTable(response);
            });
    });

    $("#btnMain_PlayTop50UserPicked").on("click", function () {
        armaradio.masterPageWait(true);

        armaradio.masterAJAXGet({}, "Music", "GetTop50UserPickedSongs")
            .then(function (response) {
                attachListToTable(response);
            });
    });

    $("#btnMain_UploadPlaylist").on("click", function () {
        $("#txtPastedPlaylist").val("");
        $("#dvPopupPastePlaylist").modal("show");
    });

    $("#btnPopup_Apply").on("click", function () {
        let playlistTxt = $.trim($("#txtPastedPlaylist").val());

        if (playlistTxt != "") {
            if (playlistTxt.indexOf("|") == -1) {
                armaradio.warningMsg({
                    msg: "\"Artists\", \"Songs\" need to be separated by \"|\"",
                    captionMsg: "Required",
                    typeLayout: "red"
                });
            } else {
                armaradio.masterPageWait(true);

                armaradio.masterAJAXPost({
                    PlayList: playlistTxt
                }, "Music", "UploadCustomPlaylist")
                    .then(function (response) {
                        $("#dvPopupPastePlaylist").modal("hide");
                        $("#txtPastedPlaylist").val("");

                        attachListToTable(response);
                    });
            }
        }
    });

    $("#txtMainGeneralSearch").on("keyup", function (e) {
        if (e.keyCode === 13) {
            performGeneralSearch();
        }
    });

    $("#btnMain_GeneralSearch").on("click", function () {
        performGeneralSearch();
    });

    //armaradio.masterAJAXGet({
    //    search: ""
    //}, "Music", "GetArtistList")
    //    .then(function (response) {
    //        if (response && response.length) {
                
    //        }
    //    });
}

function performGeneralSearch() {
    let searchText = $.trim($("#txtMainGeneralSearch").val());

    if (searchText != "") {
        armaradio.masterPageWait(true);

        armaradio.masterAJAXGet({
            SearchText: searchText
        }, "Music", "GeneralSearch")
            .then(function (response) {
                $("#dvPopupPastePlaylist").modal("hide");
                $("#txtPastedPlaylist").val("");

                attachListToTableFromGeneralSearch(response);
            });
    }
}

function attachListToTable(response, isPageLoad) {
    if (response && response.length) {
        let tblPlaylist = $("<table id=\"tblMainPlayList\"></table>");

        for (let i = 0; i < response.length; i++) {
            tblPlaylist.append(
                $("<tr></tr>").attr({
                    "data-tid": response[i].tid,
                    "data-artist": response[i].artistName,
                    "data-song": response[i].songName
                })
            );

            tblPlaylist.find("tr").last().append(
                $("<td></td>").append(
                    $("<div class=\"div-thumbnail\"></div>")
                        .attr("style", "background: url(https://picsum.photos/id/" + Math.floor(Math.random() * 900) + "/65/50) no-repeat center center / cover;")
                )
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").html(response[i].artistName)
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").html(response[i].songName)
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").append($("<button class=\"btn btn-primary font-sz-0 pt-0 pb-0 btn-play-top-song\"><span class=\"font-sz-11pt\">Play</span></button>"))
            );
        }

        $("#tblMainPlayList").replaceWith(tblPlaylist);
        topSongsAttachClickEvents(!isPageLoad ? true : false);
    } else {
        armaradio.masterPageWait(false);
    }
}

function attachListToTableFromGeneralSearch(response) {
    if (response && response.length) {
        let tblPlaylist = $("<table id=\"tblMainPlayList\"></table>");

        for (let i = 0; i < response.length; i++) {
            tblPlaylist.append(
                $("<tr></tr>").attr({
                    "data-tid": "-1",
                    "data-artist": response[i].artistName,
                    "data-song": response[i].songName,
                    "data-videoid": response[i].videoId,
                    "data-thumbnailurl": JSON.stringify(response[i].thumbNail || "")
                })
            );

            tblPlaylist.find("tr").last().append(
                $("<td></td>").append(
                    $("<div class=\"div-thumbnail\"></div>")
                        .attr("style", "background: url(https://picsum.photos/id/" + Math.floor(Math.random() * 900) + "/65/50) no-repeat center center / cover;")
                )
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").html(response[i].artistName)
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").html(response[i].songName)
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").append($("<button class=\"btn btn-primary font-sz-0 pt-0 pb-0 btn-play-top-song\"><span class=\"font-sz-11pt\">Play</span></button>"))
            );
        }

        $("#tblMainPlayList").replaceWith(tblPlaylist);
        topSongsAttachClickEvents(true);
    } else {
        armaradio.masterPageWait(false);
    }
}

function topSongsAttachClickEvents(startPlaying) {
    $("#tblMainPlayList").find("button.btn-play-top-song").each(function () {
        $(this).on("click", function () {
            armaradio.masterPageWait(true);

            let currentRow = $(this).closest("tr");
            let artistName = $.trim(currentRow.attr("data-artist"));
            let songName = $.trim(currentRow.attr("data-song"));
            let videoId = $.trim(currentRow.attr("data-videoid"));

            $("#tblMainPlayList").find("tr.now-playing").removeClass("now-playing");
            currentRow.addClass("now-playing");

            if (videoId != "") {
                let newIframe = $("<iframe></iframe");
                newIframe.attr({
                    "id": "armaMainPlayer",
                    "class": "iframe-holder",
                    "src": "https://www.youtube.com/embed/" + videoId + "?enablejsapi=1",
                    "width": "356", //560
                    "height": "200", //315
                    "frameborder": "0",
                    "allow": "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share",
                    "allowfullscreen": ""
                });

                $("#dvMainPlay_currentlyPlaying").find(".iframe-holder").remove();
                $("#dvMainPlay_currentlyPlaying").append(newIframe);

                let player = new YT.Player("armaMainPlayer", {
                    events: {
                        "onReady": onPlayerReady,
                        "onStateChange": onPlayerStateChange,
                        "onError": onPlayerError
                    }
                });

                armaradio.masterPageWait(false);
            } else {
                armaradio.masterAJAXPost({
                    artistName: artistName,
                    songName: songName
                }, "Music", "GetUrlByArtistSongName")
                    .then(function (response) {
                        if (response) {
                            if (response.hasVideo) {
                                currentRow.attr("data-videoid", response.videoId);

                                let newIframe = $("<iframe></iframe");
                                newIframe.attr({
                                    "id": "armaMainPlayer",
                                    "class": "iframe-holder",
                                    "src": response.embedUrl,
                                    "width": "356", //560
                                    "height": "200", //315
                                    "frameborder": "0",
                                    "allow": "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share",
                                    "allowfullscreen": ""
                                });

                                $("#dvMainPlay_currentlyPlaying").find(".iframe-holder").remove();
                                $("#dvMainPlay_currentlyPlaying").append(newIframe);

                                let player = new YT.Player("armaMainPlayer", {
                                    events: {
                                        "onReady": onPlayerReady,
                                        "onStateChange": onPlayerStateChange,
                                        "onError": onPlayerError
                                    }
                                });

                                armaradio.masterPageWait(false);
                            } else {
                                let newIframe = $("<div></div");
                                newIframe.attr({
                                    "id": "armaMainPlayer",
                                    "class": "iframe-holder"
                                });

                                newIframe.append($("<span class=\"lbl-not-found\"></span>").html("Song not found"));

                                $("#dvMainPlay_currentlyPlaying").find(".iframe-holder").remove();
                                $("#dvMainPlay_currentlyPlaying").append(newIframe);

                                setTimeout(function () {
                                    playerPlayNext();
                                }, 1500);

                                armaradio.masterPageWait(false);
                            }
                        }
                    });
            }
        });
    });

    if (startPlaying) {
        $("#tblMainPlayList").find("button.btn-play-top-song").first().trigger("click");
    }

    armaradio.masterPageWait(false);
}

function onPlayerReady(e) {
    e.target.playVideo();
}

function onPlayerStateChange(e) {
    if (e.data == YT.PlayerState.ENDED) {
        playerPlayNext();
    } else {
        console.log(e);
    }
}

function onPlayerError(e) {
    console.log(e);

    playerPlayNext(true);
}

function playerPlayNext(fromError) {
    let lastPlayedRow = $("#tblMainPlayList").find("tr.now-playing");
    let nextPlay;

    if (lastPlayedRow.length) {
        if (fromError) {
            lastPlayedRow.attr("data-videoid", "");
        }

        let indexRow = lastPlayedRow.index() + 1;
        nextPlay = $("#tblMainPlayList").find("tr").eq(indexRow);

        if (nextPlay.length) {
            $("#tblMainPlayList").find("tr.now-playing").removeClass("now-playing");
            nextPlay.addClass("now-playing");
        }
    } else {
        $("#tblMainPlayList").find("tr.now-playing").removeClass("now-playing");
        $("#tblMainPlayList").find("tr").first().addClass("now-playing");

        nextPlay = $("#tblMainPlayList").find("tr.now-playing");
    }

    if (nextPlay.length) {
        nextPlay[0].scrollIntoView({
            behavior: "smooth",
            block: "start"
        });

        nextPlay.find("button.btn-play-top-song").trigger("click");
    }
}