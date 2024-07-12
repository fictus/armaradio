function onArmaLoaderOnload() {
    $("#btnDownloadPlaylistSongs").on("click", function () {
        $("#tblSongsList tbody tr").each(function () {
            let row = $(this);

            downloadPendingSong($.trim(row.attr("data-videoid")));
        });
    });
}

async function downloadPendingSong(videoId) {
    await DotNet.invokeMethodAsync("armaoffline", "DownloadFile", videoId);
}

function loginError(errorMsg) {
    $("#lblLoginError").html(errorMsg);
}

function populateSongsToDownloadTable(data) {
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