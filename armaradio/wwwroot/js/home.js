$(document).ready(function () {
    mainload_attacheEvents();
});

function mainload_attacheEvents() {
    $("#dvPopupPastePlaylist").modal();

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
    });
}

function attachListToTable(response) {
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
            let artistName = currentRow.attr("data-artist");
            let songName = currentRow.attr("data-song");

            $("#tblMainPlayList").find("tr.now-playing").removeClass("now-playing");
            currentRow.addClass("now-playing");

            armaradio.masterAJAXPost({
                artistName: artistName,
                songName: songName
            }, "Music", "GetUrlByArtistSongName")
                .then(function (response) {
                    if (response) {
                        if (response.hasVideo) {
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

    playerPlayNext();
}

function playerPlayNext() {
    let lastPlayedRow = $("#tblMainPlayList").find("tr.now-playing");
    let nextPlay;

    if (lastPlayedRow.length) {
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