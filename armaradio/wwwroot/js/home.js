$(document).ready(function () {
    mainload_attacheEvents();
});

function mainload_attacheEvents() {
    $("#dvPopupPastePlaylist").modal();
    $("#dvPopupLoadPlaylists").modal();

    armaradio.masterAJAXGet({}, "Music", "GetCurrentTop100")
        .then(function (response) {
            attachListToTable(response, true);
        });

    //$("#cmbMasterSearchOptions").on("hide.bs.dropdown", function (e) {
    //    console.log(e);
    //});

    $("#cmbMasterSearchOptions").find("a.dropdown-item").each(function () {
        $(this).on("click", function (e) {
            e.preventDefault();

            let currentA = $(this);

            if (!currentA.hasClass("active")) {
                $("#cmbMasterSearchOptions").find("a.dropdown-item").removeClass("active");
                currentA.addClass("active");
                let iconHtml = currentA.find("i")[0].outerHTML;

                $("#cmbMasterSearchOptions").find("button.dropdown-toggle").html(iconHtml);

                $("#txtMainGeneralSearch").val("");
                $("#txtMainGeneralSearch")[0].focus();
            }
        });
    });

    $("#btnMainPlayerToggleRepeat").on("click", function () {
        let btn = $("#btnMainPlayerToggleRepeat");
        let currentStatus = $.trim(btn.attr("data-status"));

        if (currentStatus == "1") {
            btn.attr({
                "data-status": "0",
                "title": "repeat off"
            });
        } else {
            btn.attr({
                "data-status": "1",
                "title": "repeat on"
            });
        }
    });

    $("#chkPastePlaylist_CreateNewPlaylist").on("change", function () {
        if ($(this).is(":checked")) {
            $("#txtPasterPlaylistName").val("");
            $("#dvPastePlaylist_PlaylistNameHolder").css("display", "");
        } else {
            $("#dvPastePlaylist_PlaylistNameHolder").css("display", "none");
        }
    });

    $("#chkAddToPlaylist_CreateNewPlaylist").on("change", function () {
        $(".add-to-playlist-holder-dv").css("display", "none");

        if ($(this).is(":checked")) {
            $("#txtAddToPlaylistNewPlaylistName").val("");
            $("#dvAddToPlaylistName_Holder").css("display", "");

            $("#btnNonePlaylistOptions_AddToPlaylist").removeAttr("disabled");
        } else {
            $("#dvAddToPlaylistCmbList_Holder").css("display", "");

            $("#cmbAddToPlaylistNames").trigger("change");
        }
    });

    $("#cmbMainOptions").on("change", function () {
        let selectedId = $.trim($(this).find("option:selected").val());

        if (selectedId == "1") {
            armaradio.masterPageWait(true);

            armaradio.masterAJAXGet({}, "Music", "GetCurrentTop100")
                .then(function (response) {
                    attachListToTable(response);
                });
        } else if (selectedId == "2") {
            armaradio.masterPageWait(true);

            armaradio.masterAJAXGet({}, "Music", "GetCurrentTop40DanceSingles")
                .then(function (response) {
                    attachListToTable(response);
                });
        } else if (selectedId == "3") {
            armaradio.masterPageWait(true);

            armaradio.masterAJAXGet({}, "Music", "GetCurrentTranceTop100")
                .then(function (response) {
                    attachListToTable(response);
                });
        } else if (selectedId == "4") {
            armaradio.masterPageWait(true);

            armaradio.masterAJAXGet({}, "Music", "GetCurrentTranceHype100")
                .then(function (response) {
                    attachListToTable(response);
                });
        }
        //else if (selectedId == "3") {
        //    if ($("#dvPopupLoadPlaylists").length) {
        //        $("#dvPopupLoadPlaylists").modal("show");

        //        $("#cmbMainOptions").val("1"); //("option").first().attr("selected", "selected");
        //    }
        //}
    });

    $("#btnMain_LoadPlaylist").on("click", function () {
        if ($("#lnkMainLogin").length) {
            armaradio.warningMsg({
                msg: "Please login or create a free account",
                captionMsg: "Login Required",
                typeLayout: "red"
            });
        } else {
            if ($("#dvPopupLoadPlaylists").length) {
                $("#dvPopupLoadPlaylists").modal("show");
            }
        }
        //$("#txtPastedPlaylist").val("");
        //$("#chkPastePlaylist_CreateNewPlaylist").prop("checked", false);
        //$("#chkPastePlaylist_CreateNewPlaylist").trigger("change");

        //$("#dvPopupPastePlaylist").modal("show");
    });

    $("#btnPopup_Apply").on("click", function () {
        let playlistTxt = $.trim($("#txtPastedPlaylist").val());
        let playlistName = $.trim($("#txtPasterPlaylistName").val());
        let createNewPlaylist = ($("#chkPastePlaylist_CreateNewPlaylist").length ? $("#chkPastePlaylist_CreateNewPlaylist").is(":checked") : false);

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
                    PlayList: playlistTxt,
                    PlaylistName: playlistName,
                    CreateNewPlaylist: createNewPlaylist
                }, "Music", "UploadCustomPlaylist")
                    .then(function (response) {
                        if (response && response.error) {
                            armaradio.masterPageWait(false);

                            armaradio.warningMsg({
                                msg: response.errorMsg,
                                captionMsg: "Error",
                                typeLayout: "red"
                            });
                        } else {
                            $("#dvPopupPastePlaylist").modal("hide");
                            $("#txtPastedPlaylist").val("");

                            if (response.playlistId) {
                                attachListToTable(response.songList, null, {
                                    playlistId: response.playlistId,
                                    playlistTitle: response.playlistName
                                });
                            } else {
                                attachListToTable(response.songList);
                            }
                        }
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

    $("#cmbLoadPlaylistNames").on("change", function () {
        let optionsSelected = $(this).find("option:selected");

        if (optionsSelected.length) {
            $("#btnPopupLoadPlaylist").removeAttr("disabled");
        } else {
            $("#btnPopupLoadPlaylist").attr("disabled", "disabled");
        }
    });

    $("#dvPopupLoadPlaylists").on("show.bs.modal", function (e) {
        armaradio.masterPageWait(true);

        $("#btnPopupLoadPlaylist").attr("disabled", "disabled");
        $("#cmbLoadPlaylistNames").find("option").remove();

        armaradio.masterAJAXGet({}, "Music", "GetUserPlaylists")
            .then(function (response) {
                if (response && response.error) {
                    armaradio.masterPageWait(false);

                    armaradio.warningMsg({
                        msg: response.errorMsg,
                        captionMsg: "Error",
                        typeLayout: "red"
                    });
                } else {
                    if (response && response.length) {
                        for (let i = 0; i < response.length; i++) {
                            $("#cmbLoadPlaylistNames").append(
                                $("<option></option>")
                                    .attr("value", response[i].id)
                                    .html(response[i].playlistName)
                            );
                        }
                    }

                    $("#cmbLoadPlaylistNames").trigger("change");

                    armaradio.masterPageWait(false);
                }
            });
    });

    $("#btnPopupLoadPlaylist").on("click", function () {
        let selectedPlaylistId = $.trim($("#cmbLoadPlaylistNames option:selected").val());
        let selectedPlayName = $.trim($("#cmbLoadPlaylistNames option:selected").text());

        if (selectedPlaylistId != "") {
            armaradio.masterPageWait(true);

            armaradio.masterAJAXGet({
                PlaylistId: selectedPlaylistId
            }, "Music", "LoadUserSelectedPlaylist")
                .then(function (response) {
                    $("#dvPopupLoadPlaylists").modal("hide");

                    attachListToTable(response, null, {
                        playlistId: selectedPlaylistId,
                        playlistTitle: selectedPlayName
                    });
                });
        }
    });

    $("#cmbAddToPlaylistNames").on("change", function () {
        let optionsSelected = $(this).find("option:selected");

        if (optionsSelected.length) {
            $("#btnNonePlaylistOptions_AddToPlaylist").removeAttr("disabled");
        } else {
            $("#btnNonePlaylistOptions_AddToPlaylist").attr("disabled", "disabled");
        }
    });

    $("#offcanvasNonePlaylistOptions").on("show.bs.offcanvas", function () {
        armaradio.masterPageWait(true);

        $("#chkAddToPlaylist_CreateNewPlaylist").prop("checked", false);
        $("#chkAddToPlaylist_CreateNewPlaylist").trigger("change");

        $("#btnNonePlaylistOptions_AddToPlaylist").attr("disabled", "disabled");
        $("#cmbAddToPlaylistNames").find("option").remove();

        armaradio.masterAJAXGet({}, "Music", "GetUserPlaylists")
            .then(function (response) {
                if (response && response.error) {
                    armaradio.masterPageWait(false);

                    armaradio.warningMsg({
                        msg: response.errorMsg,
                        captionMsg: "Error",
                        typeLayout: "red"
                    });
                } else {
                    if (response && response.length) {
                        for (let i = 0; i < response.length; i++) {
                            $("#cmbAddToPlaylistNames").append(
                                $("<option></option>")
                                    .attr("value", response[i].id)
                                    .html(response[i].playlistName)
                            );
                        }
                    }

                    $("#cmbAddToPlaylistNames").trigger("change");

                    armaradio.masterPageWait(false);
                }
            });
    });

    $("#offcanvasNonePlaylistOptions").on("hidden.bs.offcanvas", function () {
        $("#btnNonePlaylistOptions_AddToPlaylist").attr({
            "data-artist": "",
            "data-song": "",
            "data-videoid": ""
        });
    });

    $("#btnNonePlaylistOptions_AddToPlaylist").on("click", function () {
        let currentPlaylistId = $.trim($("#cmbAddToPlaylistNames").find("option:selected").val());
        let currentArtist = $.trim($("#btnNonePlaylistOptions_AddToPlaylist").attr("data-artist"));
        let currentSong = $.trim($("#btnNonePlaylistOptions_AddToPlaylist").attr("data-song"));
        let currentVideoId = $.trim($("#btnNonePlaylistOptions_AddToPlaylist").attr("data-videoid"));
        let addToNewPlaylist = $("#chkAddToPlaylist_CreateNewPlaylist").is(":checked");
        let newPlaylistName = $.trim($("#txtAddToPlaylistNewPlaylistName").val());

        if (addToNewPlaylist) {
            if (newPlaylistName == "") {
                armaradio.warningMsg({
                    msg: "'Playlist Name' is required",
                    captionMsg: "Error",
                    typeLayout: "red"
                });
            } else {
                if ((currentArtist != "" || currentSong != "")) {
                    armaradio.masterPageWait(true);

                    armaradio.masterAJAXPost({
                        PlaylistId: -1,
                        PlaylistName: newPlaylistName,
                        Artist: currentArtist,
                        Song: currentSong,
                        VideoId: currentVideoId
                    }, "Music", "AddSongToNewPlaylist")
                        .then(function (response) {
                            if (response && response.error) {
                                armaradio.masterPageWait(false);

                                armaradio.warningMsg({
                                    msg: response.errorMsg,
                                    captionMsg: "Error",
                                    typeLayout: "red"
                                });
                            } else {
                                $("#offcanvasNonePlaylistOptions").find("button.btn-close").trigger("click");

                                armaradio.masterPageWait(false);
                            }
                        });
                }
            }
        } else {
            if (currentPlaylistId != "" && (currentArtist != "" || currentSong != "")) {
                armaradio.masterPageWait(true);

                armaradio.masterAJAXPost({
                    PlaylistId: currentPlaylistId,
                    PlaylistName: "",
                    Artist: currentArtist,
                    Song: currentSong,
                    VideoId: currentVideoId
                }, "Music", "AddSongToPlaylist")
                    .then(function (response) {
                        if (response && response.error) {
                            armaradio.masterPageWait(false);

                            armaradio.warningMsg({
                                msg: response.errorMsg,
                                captionMsg: "Error",
                                typeLayout: "red"
                            });
                        } else {
                            $("#offcanvasNonePlaylistOptions").find("button.btn-close").trigger("click");

                            armaradio.masterPageWait(false);
                        }
                    });
            }
        }
    });

    if ($("#btnSongOptions_RemoveFromPlaylist").length) {
        let removeButton = $("<button id='btnRemoveSongFromPlaylistConfirmed' class='btn btn-danger'>Remove</button>");
        removeButton.on("click", function () {
            let currentSongId = $.trim($("#btnSongOptions_RemoveFromPlaylist").attr("data-id"));

            if (currentSongId != "") {
                armaradio.masterPageWait(true);

                armaradio.masterAJAXGet({
                    SongId: currentSongId
                }, "Music", "DeleteSongFromPlaylist")
                    .then(function (response) {
                        if (response && response.error) {
                            armaradio.masterPageWait(false);

                            armaradio.warningMsg({
                                msg: response.errorMsg,
                                captionMsg: "Error",
                                typeLayout: "red"
                            });
                        } else {
                            $("#tblMainPlayList").find("tr[data-tid='" + currentSongId + "']").remove();

                            var popover = bootstrap.Popover.getInstance($("#btnSongOptions_RemoveFromPlaylist")[0]);
                            popover.hide();

                            $("#offcanvasSongOptions").find("button.btn-close").trigger("click");

                            armaradio.masterPageWait(false);
                        }
                    });
            }
        });

        let popoverContents = $("<div></div>");
        popoverContents.append(
            $("<div></div>")
                .html("Are you sure you want to remove this song from your playlist?")
        );
        popoverContents.append(
            $("<div class='mt-4 align-right'></div>")
                .append(removeButton)
        );

        let confirmDeleteFromPlaylist = new bootstrap.Popover($("#btnSongOptions_RemoveFromPlaylist")[0], {
            html: true,
            content: popoverContents
        });
    }

    $("#offcanvasSongOptions").on("hide.bs.offcanvas", function () {
        $("#btnSongOptions_RemoveFromPlaylist").attr("data-id", "");

        var popover = bootstrap.Popover.getInstance($("#btnSongOptions_RemoveFromPlaylist")[0]);
        popover.hide();
    });
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

function attachListToTable(response, isPageLoad, loadedPlaylist) {
    if (loadedPlaylist) {
        $("#lblTblHeaderPlaylistName").attr("data-playlistid", loadedPlaylist.playlistId);
        $("#lblTblHeaderPlaylistName").html("Playlist: " + loadedPlaylist.playlistTitle);
    } else {
        $("#lblTblHeaderPlaylistName")
            .attr("data-playlistid", "")
            .html("");
    }

    if (response && response.length) {
        let tblPlaylist = $("<table id=\"tblMainPlayList\"></table>");

        for (let i = 0; i < response.length; i++) {
            tblPlaylist.append(
                $("<tr></tr>").attr({
                    "data-tid": response[i].tid,
                    "data-artist": response[i].artistName,
                    "data-song": response[i].songName,
                    "data-videoid": response[i].videoId
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
                $("<td></td>").append($("<div class=\"row-actions-cotrols\"></div>"))
            );
            tblPlaylist.find("tr").last().find("td").last().find("div").append(
                $("<button class=\"btn btn-primary font-sz-0 pt-0 pb-0 btn-play-inner-btn-play mr-3\"><span class=\"font-sz-11pt\">Play</span></button>")
            );
            if (loadedPlaylist) {
                tblPlaylist.find("tr").last().find("td").last().find("div").append(
                    $("<a class=\"font-sz-11pt btn-inner-more-options\"><i class='fas fa-ellipsis-v pl-2 pr-2'></i></a>")
                );
            } else {
                if ($("#offcanvasNonePlaylistOptions").length) {
                    tblPlaylist.find("tr").last().find("td").last().find("div").append(
                        $("<a class=\"font-sz-11pt btn-inner-more-none-list-options\"><i class='fas fa-ellipsis-v pl-2 pr-2'></i></a>")
                    );
                }
            }
        }

        $("#tblMainPlayList").replaceWith(tblPlaylist);
        rowSongsAttachClickEvents((!isPageLoad ? true : false), loadedPlaylist);
    } else {
        armaradio.masterPageWait(false);
    }
}

function attachListToTableFromGeneralSearch(response) {
    $("#lblTblHeaderPlaylistName")
        .attr("data-playlistid", "")
        .html("Search Results");

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
                $("<td></td>").append($("<div class=\"row-actions-cotrols\"></div>"))
            );
            tblPlaylist.find("tr").last().find("td").last().find("div").append(
                $("<button class=\"btn btn-primary font-sz-0 pt-0 pb-0 btn-play-inner-btn-play mr-3\"><span class=\"font-sz-11pt\">Play</span></button>")
            );
            if ($("#offcanvasNonePlaylistOptions").length) {
                tblPlaylist.find("tr").last().find("td").last().find("div").append(
                    $("<a class=\"font-sz-11pt btn-inner-more-none-list-options\"><i class='fas fa-ellipsis-v pl-2 pr-2'></i></a>")
                );
            }
        }

        $("#tblMainPlayList").replaceWith(tblPlaylist);
        rowSongsAttachClickEvents(true);
    } else {
        armaradio.masterPageWait(false);
    }
}

function rowSongsAttachClickEvents(startPlaying, fromPlaylist) {
    $("#tblMainPlayList").find("button.btn-play-inner-btn-play").each(function () {
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

    if (fromPlaylist) {
        $("#tblMainPlayList").find("a.btn-inner-more-options").each(function () {
            $(this).on("click", function (e) {
                let currentRow = $(this).closest("tr");
                let songId = $.trim(currentRow.attr("data-tid"));
                let artistName = $.trim(currentRow.attr("data-artist"));
                let songName = $.trim(currentRow.attr("data-song"));
                let separator = (artistName != "" && songName != "" ? " - " : "");

                if (songId != "") {
                    $("#btnSongOptions_RemoveFromPlaylist").attr("data-id", songId);
                    $("#lblSongOptionsTitle").html(artistName + separator + songName);

                    let bsOffcanvas = new bootstrap.Offcanvas($("#offcanvasSongOptions")[0]);
                    bsOffcanvas.show();
                }
            });
        });
    }

    if ($("#offcanvasNonePlaylistOptions").length) {
        $("#tblMainPlayList").find("a.btn-inner-more-none-list-options").each(function () {
            $(this).on("click", function (e) {
                let currentRow = $(this).closest("tr");
                let artistName = $.trim(currentRow.attr("data-artist"));
                let songName = $.trim(currentRow.attr("data-song"));
                let videoId = $.trim(currentRow.attr("data-videoid"));
                let separator = (artistName != "" && songName != "" ? " - " : "");

                $("#btnNonePlaylistOptions_AddToPlaylist").attr({
                    "data-artist": artistName,
                    "data-song": songName,
                    "data-videoid": videoId
                });
                $("#lblNonePlaylistOptionsTitle").html(artistName + separator + songName);

                let bsOffcanvas = new bootstrap.Offcanvas($("#offcanvasNonePlaylistOptions")[0]);
                bsOffcanvas.show();
            });
        });
    }

    if (startPlaying) {
        $("#tblMainPlayList").find("button.btn-play-inner-btn-play").first().trigger("click");
    }

    armaradio.masterPageWait(false);
}

function onPlayerReady(e) {
    e.target.playVideo();
}

function onPlayerStateChange(e) {
    if (e.data == YT.PlayerState.ENDED) {
        let currentStatus = $.trim($("#btnMainPlayerToggleRepeat").attr("data-status"));

        if (currentStatus == "1") {
            e.target.playVideo();
        } else {
            playerPlayNext();
        }
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

        if ($("button.btn-play-inner-btn-play").length) {
            nextPlay.find("button.btn-play-inner-btn-play").trigger("click");
        }
    }
}