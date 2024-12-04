function onArmaLoaderOnload() {
    $(document).ready(function () {
        disableBackButton();
    });
}

function disableBackButton() {
    // Prevent default back navigation
    history.pushState(null, null, location.href);
    window.onpopstate = function () {
        history.pushState(null, null, location.href);
    };

    // Additional Android-specific back button prevention
    document.addEventListener("backbutton", function (e) {
        e.preventDefault();
    }, false);
}

function downloadSelectedPlaylistSongs(playListId) {
    if ($.trim(playListId) != "") {
        if ($("#tblSongsList tbody tr").length) {
            $("#tblSongsList tbody tr").each(function () {
                let row = $(this);

                downloadPendingSong($.trim(row.attr("data-videoid")));
            });

            markPlaylistSongsAsDownloaded(parseInt(playListId));
        }
    }
}

function onOfflinePlayerOnload() {
    $("#sldPlayerLocation").on("input", function (e) {
        let newPlaybackPosition = parseFloat(e.target.value);

        setAudioToPosition(newPlaybackPosition);
    });
}

function showPageWait(show) {
    armaradio.masterPageWait(show);
}

async function refreshArmaLoaderUI() {
    await DotNet.invokeMethodAsync("armaoffline", "RefreshUI");
}

async function downloadPendingSong(videoId) {
    await DotNet.invokeMethodAsync("armaoffline", "DownloadFile", videoId);
}

async function markPlaylistSongsAsDownloaded(playListId) {
    await DotNet.invokeMethodAsync("armaoffline", "MarkPlaylistSongsAsDownloaded", playListId);
}

function loginError(errorMsg) {
    $("#lblLoginError").html(errorMsg);
}

function populateSongsToDownloadTable(data, playListId) {
    $("#btnDownloadPlaylistSongs").attr("curent-playlistid", "");

    if (playListId) {
        $("#btnDownloadPlaylistSongs").attr("curent-playlistid", playListId);
    }

    if ((data || []).length) {
        $("#btnDownloadPlaylistSongs").removeAttr("disabled");
    } else {
        $("#btnDownloadPlaylistSongs").attr("disabled", "disabled");
    }

    let tbl = $("#tblSongsList");
    tbl.find("tbody").find("tr").remove();

    for (let i = 0; i < data.length; i++) {
        let songInfo = [];

        if ($.trim(data[i].artist) != "") {
            songInfo.push(data[i].artist);
        }
        if ($.trim(data[i].song) != "") {
            songInfo.push(data[i].song);
        }

        let currentTr = $("<tr></tr>")
            .attr({
                //"class": "table-primary",
                "data-videoid": data[i].videoId,
                "data-artist": data[i].artist,
                "data-song": data[i].song
            })
            .html(songInfo.join(" - "));

        tbl.find("tbody").append(currentTr);
    }
}

function populateSongsToPlayerTable(data, playlistTitle) {
    $("#lblTblHeaderPlaylistName").html(playlistTitle);

    let tblPlaylist = $("#tblMainPlayList");
    tblPlaylist.find("tbody").find("tr").remove();

    if ((data || []).length) {
        for (let i = 0; i < data.length; i++) {
            tblPlaylist.append(
                $("<tr></tr>").attr({
                    "data-tid": "",
                    "data-artist": data[i].artist,
                    "data-song": data[i].song,
                    "data-videoid": data[i].videoId
                })
            );

            tblPlaylist.find("tr").last().append(
                $("<td></td>").append(
                    $("<div class=\"div-thumbnail-holder\"></div>").append(
                        $("<div class=\"div-thumbnail\"></div>")
                            .attr("style", "background: url(../images/79.jpg) no-repeat center center / cover;")
                    )
                )
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").html(data[i].artist)
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").html(data[i].song)
            );
            tblPlaylist.find("tr").last().append(
                $("<td></td>").append($("<div class=\"row-actions-cotrols\"></div>"))
            );
            tblPlaylist.find("tr").last().find("td").last().find("div").append(
                $("<button class=\"btn btn-primary font-sz-0 pt-0 pb-0 btn-play-inner-btn-play mr-3\"><span class=\"font-sz-11pt\">Play</span></button>")
            );
        }

        $("#tblMainPlayList").find("button.btn-play-inner-btn-play").each(function () {
            $(this).on("click", function () {
                preparePlayNowRow(this);
            });
        });
    }
}

function preparePlayNowRow(btn) {
    //armaradio.masterPageWait(true);

    let currentRow = $(btn).closest("tr");
    let artistName = $.trim(currentRow.attr("data-artist"));
    let songName = $.trim(currentRow.attr("data-song"));
    let videoId = $.trim(currentRow.attr("data-videoid"));

    $("#tblMainPlayList").find("tr.now-playing").removeClass("now-playing");
    currentRow.addClass("now-playing");

    if (videoId != "") {
        $("a.lnk-attribution-notice").attr("data-artistname", artistName);
        $("a.lnk-attribution-notice").attr("data-songname", songName);
        //$("a.lnk-attribution-notice").attr("data-url", "https://www.youtube.com/watch?v=" + videoId);

        playSong(videoId);

        //armaradio.masterPageWait(false);
    }
}

function playNextAvailableSong() {
    let currentIndex = $("#tblMainPlayList").find("tr.now-playing").index() + 1;
    let nextSongTr = $("#tblMainPlayList").find("tr").eq(currentIndex);

    if (nextSongTr.length) {
        let playButton = nextSongTr.find("button.btn-play-inner-btn-play");

        preparePlayNowRow(playButton);
    } else {
        $("#tblMainPlayList").find("tr.now-playing").removeClass("now-playing");

        clearPlayer();
    }
}

async function playSong(videoId) {
    await DotNet.invokeMethodAsync("armaoffline", "Play", videoId);
}

async function clearPlayer() {
    await DotNet.invokeMethodAsync("armaoffline", "Play", "");
}

async function setAudioToPosition(position) {
    await DotNet.invokeMethodAsync("armaoffline", "SetAudioToPosition", position);
}

function updatePlayerSliderPosition(position) {
    console.log(position);
    $("#sldPlayerLocation").val(position);
}

function presetOfflineBtnValues(playlistId) {
    if ($.trim(playlistId) != "") {
        let optioName = $("#cmbOfflinePlaylists option[value='" + playlistId + "']");

        returnSelectedOfflinePlaylistValues(parseInt(playlistId), optioName.text());
    } else {
        returnSelectedOfflinePlaylistValues(null, "");
    }
}

async function returnSelectedOfflinePlaylistValues(playlistId, name) {
    await DotNet.invokeMethodAsync("armaoffline", "SetSelectedOfflinePlaylistValues", playlistId, name);
}

var armaradio = {
    /************************************************
        masterPageWait
    ************************************************/
    masterPageWait: function (show) {
        if (show) {
            $("div.master-page-wait").css("display", "");
        } else {
            $("div.master-page-wait").css("display", "none");
        }
    }
};