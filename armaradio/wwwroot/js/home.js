var arma_mainSearchSelectedType = "1";
var localHomePlayer;
var soundWaveColor = "#E14B4B";

$(document).ready(function () {
    mainload_attacheEvents();

    if ("radioplayer_attachEvents" in window) {
        radioplayer_attachEvents();
    }
});

function mainload_attacheEvents() {
    $("#dvPopupPastePlaylist").modal();
    $("#dvPopupLoadPlaylists").modal();
    $("#dvPopupRefineRadioParameters").modal();

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
                arma_mainSearchSelectedType = $.trim(currentA.attr("data-type"));

                $("#cmbMasterSearchOptions").find("a.dropdown-item").removeClass("active");
                currentA.addClass("active");
                let iconHtml = currentA.find("i")[0].outerHTML;

                $("#cmbMasterSearchOptions").find("button.dropdown-toggle").html(iconHtml);

                $("#ulArtistsFound").css("display", "none");
                $("#btnArtistAlbumsOpen").css("display", "none");

                //$("#ulArtistsFound").find("li").remove();

                if (arma_mainSearchSelectedType == "1") {
                    $("#txtMainGeneralSearch").css("display", "");
                    $("#txtMainGeneralSearch").attr("placeholder", "Artist Search");
                    $("#btnMain_GeneralSearch").css("display", "none");
                } else if (arma_mainSearchSelectedType == "2") {
                    $("#txtMainGeneralSearch").css("display", "");
                    $("#txtMainGeneralSearch").attr("placeholder", "General Search");
                    $("#btnMain_GeneralSearch").css("display", "");
                } else if (arma_mainSearchSelectedType == "3") {
                    if ($("#lnkMainLogin").length) {
                        armaradio.warningMsg({
                            msg: "Radio Player requires you to login or create a free account",
                            captionMsg: "Login Required",
                            typeLayout: "red"
                        });
                    } else {
                        if ("showRadioSearchBox" in window) {
                            showRadioSearchBox();
                        }
                    }

                    $("#cmbMasterSearchOptions").find("a.dropdown-item[data-type='1']").trigger("click");
                }

                $("#txtMainGeneralSearch")[0].focus();
            }
        });
    });

    let mainSearchDeff;
    $("#txtMainGeneralSearch").on("keydown", function (e) {
        if (arma_mainSearchSelectedType == "1") {
            if (mainSearchDeff) {
                try {
                    mainSearchDeff.cancel();
                } catch {

                }
            }

            clearTimeout($("#txtMainGeneralSearch").data("timeout"));

            $("#txtMainGeneralSearch").data("timeout", setTimeout(function () {
                let searchPhrase = $("#txtMainGeneralSearch").val();

                if ($.trim(searchPhrase) != "") {
                    mainSearchDeff = armaradio.masterAJAXPost({
                        SearchPhrase: searchPhrase
                    }, "Music", "FindArtists")
                        .then(function (response) {
                            if (response && response.error) {
                                
                            } else {
                                attachArtistResponseFromSearch(response || []);
                            }
                        });
                } else {
                    attachArtistResponseFromSearch([]);
                }
            }, 700));
        }
    });

    $("#txtMainGeneralSearch").on("focus", function () {
        if (arma_mainSearchSelectedType == "1" && $("#ulArtistsFound").find("li").length) {
            setTimeout(function () {
                $("#ulArtistsFound").css("display", "");
            }, 10);
        }
    });

    $("html, body").on("click", function (e) {
        if (!(e.target.id == "ulArtistsFound" || e.target.id == "txtMainGeneralSearch")) {
            $("#ulArtistsFound").css("display", "none");
        }
    });

    $("#btnArtistAlbumsOpen").on("click", function () {
        let isVisible = $("#offcanvasArtistAlbums").hasClass("show");
        let bsOffcanvas = new bootstrap.Offcanvas($("#offcanvasArtistAlbums")[0]);

        if (!isVisible) {
            bsOffcanvas.show();
        }
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

    $("#offcanvasNonePlaylistOptionsRightLabel").on("click", function () {
        if (!$("#lnkMainLogin").length) {
            clearTimeout($("#offcanvasNonePlaylistOptionsRightLabel").data("clicksTimeout"));

            $("#offcanvasNonePlaylistOptionsRightLabel").data("clicksTimeout", setTimeout(function () {
                $("#offcanvasNonePlaylistOptionsRightLabel").attr("data-clicks", "0");
            }, 3000));

            let currentClicks = parseInt($("#offcanvasNonePlaylistOptionsRightLabel").attr("data-clicks")) + 1;
            $("#offcanvasNonePlaylistOptionsRightLabel").attr("data-clicks", currentClicks);

            if (currentClicks > 5) {
                $("#btnPlaylist_DownloadSong").css("display", "");
            }
        }
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
    $("#offcanvasSongOptions").on("show.bs.offcanvas", function () {
        if ($(document).data("allowDownload")) {
            $(".btn-options-download").css("display", "");
        }
    });

    $("#offcanvasSongOptions").on("hidden.bs.offcanvas", function () {
        $(".btn-options-download").attr({
            "data-artist": "",
            "data-song": "",
            "data-videoid": ""
        });
        $(".btn-options-download").css("display", "none");
    });

    $("#offcanvasNonePlaylistOptions").on("show.bs.offcanvas", function () {
        armaradio.masterPageWait(true);

        if ($(document).data("allowDownload")) {
            $(".btn-options-download").css("display", "");
        }

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

        $(".btn-options-download").attr({
            "data-artist": "",
            "data-song": "",
            "data-videoid": ""
        });
        $(".btn-options-download").css("display", "none");
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

    $("#btnLoadPL_StartRadioFromPlaylistsSongs").on("click", function () {
        if ($("#lnkMainLogin").length) {
            armaradio.warningMsg({
                msg: "Radio Player requires you to login or create a free account",
                captionMsg: "Login Required",
                typeLayout: "red"
            });
        } else {
            if ("loadRadioPlayer" in window) {
                loadRadioPlayer(null, null, false, false, true);
            }
        }
    });

    $("#btnMain_StartRadioSession").on("click", function () {
        if ($("#lnkMainLogin").length) {
            armaradio.warningMsg({
                msg: "Radio Player requires you to login or create a free account",
                captionMsg: "Login Required",
                typeLayout: "red"
            });
        } else {
            if ("loadRadioPlayer" in window) {
                let artistName = $.trim($("#btnMain_StartRadioSession").attr("data-artistname"));

                loadRadioPlayer(artistName, "");
            }
        }
    });

    $("#btnPlaylist_StartRadioSession").on("click", function () {
        if ($("#lnkMainLogin").length) {
            armaradio.warningMsg({
                msg: "Radio Player requires you to login or create a free account",
                captionMsg: "Login Required",
                typeLayout: "red"
            });
        } else {
            if ("loadRadioPlayer" in window) {
                let dataHolder = $("#btnNonePlaylistOptions_AddToPlaylist");
                let artistName = $.trim(dataHolder.attr("data-artist"));
                let songName = $.trim(dataHolder.attr("data-song"));

                loadRadioPlayer(artistName, songName, true);
            }
        }
    });

    $("#btnSongOptions_StartRadioSession").on("click", function () {
        if ($("#lnkMainLogin").length) {
            armaradio.warningMsg({
                msg: "Radio Player requires you to login or create a free account",
                captionMsg: "Login Required",
                typeLayout: "red"
            });
        } else {
            if ("loadRadioPlayer" in window) {
                let dataHolder = $("#btnSongOptions_RemoveFromPlaylist");
                let artistName = $.trim(dataHolder.attr("data-artist"));
                let songName = $.trim(dataHolder.attr("data-song"));

                loadRadioPlayer(artistName, songName, true);
            }
        }
    });

    $("#lnkCloseRadio").on("click", function (e) {
        e.preventDefault();

        if (radioPlayerMain) {
            try {
                radioPlayerMain.dispose();
                radioPlayerMain = null;
            } catch (ex) {

            }
        }

        $("#dvRadioPlayerHolder").css("display", "none");
        $("#dvRadioPlayer_currentlyPlaying").find(".iframe-holder").replaceWith("<div class=\"iframe-holder pl-0 pt-0\"></div>");
    });

    $("#lnkRadioOptions").on("click", function (e) {
        e.preventDefault();

        if ("currentRadioSongOptions" in window) {
            currentRadioSongOptions();
        }
    });

    $("#lnkRadioPlayer_LikeSong").on("click", function (e) {
        e.preventDefault();

        if ("radioPlayerLikeCurrentSong" in window) {
            radioPlayerLikeCurrentSong();
        }
    });

    $("#lnkRadioPlayer_HateSong").on("click", function (e) {
        e.preventDefault();

        if ("radioPlayerHateCurrentSong" in window) {
            radioPlayerHateCurrentSong();
        }
    });

    $("#btnRefineParametersStartRadio").on("click", function () {
        if ("radioPlayerRefineParametersStartRadio" in window) {
            radioPlayerRefineParametersStartRadio();
        }
    });
}

function attachArtistResponseFromSearch(response) {
    $("#ulArtistsFound").find("li").remove();

    if (response && response.length) {

        for (let i = 0; i < response.length; i++) {
            $("#ulArtistsFound").append(
                $("<li></li>")
                    .append(
                        $("<a></a>").attr({
                            "class": "dropdown-item",
                            "href": "#",
                            "data-id": response[i].id,
                            "data-artist": response[i].artistName,
                            "data-artistflat": response[i].artistName_Flat,
                        })
                            .html(response[i].artistName)
                    )
            );
        }

        $("#ulArtistsFound").find("a").each(function () {
            $(this).on("click", function (e) {
                e.preventDefault();
                let artistName = $(this).attr("data-artistflat");
                let artistId = parseInt($(this).attr("data-id"));

                $("#txtMainGeneralSearch").val(artistName);

                //performGeneralSearch(artistName);
                getAlbumsForArtists(artistId, artistName);
            });
        });

        $("#ulArtistsFound").css("display", "");
    } else {
        $("#ulArtistsFound").css("display", "none");
    }
}

function performGeneralSearch(searchPhrase) {
    let searchType = $("#cmbMasterSearchOptions").find("a.dropdown-item.active").attr("data-type");

    if (searchType == "2") {
        let searchText = (searchPhrase || $.trim($("#txtMainGeneralSearch").val()));

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
}

function getAlbumsForArtists(artistId, artistName) {
    armaradio.masterPageWait(true);

    $("#btnArtistAlbumsOpen").css("display", "none");
    $("#btnMain_StartRadioSession")
        .attr("data-artistname", "")
        .css("display", "none");

    armaradio.masterAJAXPost({
        ArtistId: artistId
    }, "Music", "FindAlbumsForArtists")
        .then(function (response) {
            if (!(response && response.error) && (response.albums || response.singles)) {
                $("#offcanvasArtistAlbumsRightLabel").html("Albums: " + artistName);
                $("#tblArtistAlbums tr").remove();
                $("#tblArtistSingles tr").remove();

                if (response.albums) {
                    for (let i = 0; i < response.albums.length; i++) {
                        let tr = $("<tr></tr>");

                        tr.append(
                            $("<td></td>")
                                .append("<i class=\"fa-solid fa-compact-disc\"></i>")
                        );
                        tr.append(
                            $("<td></td>")
                                .append(
                                    $("<div class=\"alb-name\"></div>").html(response.albums[i].albumName)
                                )
                                .append(
                                    $("<div class=\"alb-details\"></div>").html(response.albums[i].albumDetails)
                                )
                        );
                        tr.append(
                            $("<td></td>")
                                .html(response.albums[i].releaseDate)
                        );
                        tr.append(
                            $("<td class=\"relative show-overflow\"></td>")
                                .append(
                                    $("<button class=\"btn btn-primary font-sz-0 pt-0 pb-0 btn-load-album mr-3\"></button>")
                                        .attr({
                                            "data-albumid": response.albums[i].id,
                                            "data-artistid": artistId,
                                            "data-artistname": artistName,
                                            "data-albumname": response.albums[i].albumName_Flat,
                                            "data-albumdetails": response.albums[i].albumDetails
                                        })
                                        .append("<span class=\"font-sz-11pt\">Load</span>")
                                )
                        );

                        $("#tblArtistAlbums").append(tr);
                    }
                }

                if (response.singles) {
                    for (let i = 0; i < response.singles.length; i++) {
                        let tr = $("<tr></tr>");

                        tr.append(
                            $("<td></td>")
                                .append("<i class=\"fa-solid fa-compact-disc\"></i>")
                        );
                        tr.append(
                            $("<td></td>")
                                .append(
                                    $("<div class=\"alb-name\"></div>").html(response.singles[i].albumName)
                                )
                                .append(
                                    $("<div class=\"alb-details\"></div>").html(response.singles[i].albumDetails)
                                )
                        );
                        tr.append(
                            $("<td></td>")
                                .html(response.singles[i].releaseDate)
                        );
                        tr.append(
                            $("<td class=\"relative show-overflow\"></td>")
                                .append(
                                    $("<button class=\"btn btn-primary font-sz-0 pt-0 pb-0 btn-load-album mr-3\"></button>")
                                        .attr({
                                            "data-albumid": response.singles[i].id,
                                            "data-artistid": artistId,
                                            "data-artistname": artistName,
                                            "data-albumname": response.singles[i].albumName_Flat,
                                            "data-albumdetails": response.singles[i].albumDetails
                                        })
                                        .append("<span class=\"font-sz-11pt\">Load</span>")
                                )
                        );

                        $("#tblArtistSingles").append(tr);
                    }
                }

                $("#offcanvasArtistAlbums").find("button.btn-load-album").each(function () {
                    $(this).on("click", function () {
                        let btn = $(this);
                        let artistName = btn.attr("data-artistname");
                        let artistId = btn.attr("data-artistid");
                        let albumName = btn.attr("data-albumname");
                        let albumId = btn.attr("data-albumid");
                        let albumDetails = btn.attr("data-albumdetails");
                        let albumToLoad = $.trim(artistName + " - " + albumName + " " + albumDetails);

                        $("#offcanvasArtistAlbums tr.loaded").removeClass("loaded");
                        btn.closest("tr").addClass("loaded");

                        loadAlbumSongs(artistId, albumId, artistName, albumName);

                        $("#offcanvasArtistAlbums")
                            .removeClass("show")
                            .attr({
                                "aria-hidden": "true"
                            })
                            .css("visibility", "hidden")
                            .removeAttr("aria-modal")
                            .removeAttr("role");

                        $("div.offcanvas-backdrop.fade.show").remove();
                        $("body").removeAttr("style");

                        //let bsOffcanvas = new bootstrap.Offcanvas($("#offcanvasArtistAlbums")[0]);
                        //bsOffcanvas.hide();
                    });
                });

                $("#btnArtistAlbumsOpen").css("display", "");
                $("#btnMain_StartRadioSession")
                    .attr("data-artistname", artistName)
                    .css("display", "");

                $("#offcanvasArtistAlbums div.offcanvas-body")[0].scrollTop = 0;

                $("#btnArtistAlbumsOpen").trigger("click");
            } else {
                $("#btnArtistAlbumsOpen").css("display", "none");
                $("#btnMain_StartRadioSession")
                    .attr("data-artistname", "")
                    .css("display", "none");
            }

            armaradio.masterPageWait(false);
        });
}

function loadAlbumSongs(artistId, albumId, artistName, albumTitle) {
    armaradio.masterPageWait(true);

    armaradio.masterAJAXPost({
        ArtistId: artistId,
        AlbumId: albumId
    }, "Music", "LoadAlbumSongs")
        .then(function (response) {
            let headerTitle = "Album: " + artistName + " - " + albumTitle;

            attachListToTableFromGeneralSearch(response, headerTitle);
        });
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
                    $("<a class=\"font-sz-11pt btn-inner-more-options\"><i class='fa-solid fa-ellipsis-vertical pl-2 pr-2'></i></a>")
                );
            } else {
                if ($("#offcanvasNonePlaylistOptions").length) {
                    tblPlaylist.find("tr").last().find("td").last().find("div").append(
                        $("<a class=\"font-sz-11pt btn-inner-more-none-list-options\"><i class='fa-solid fa-ellipsis-vertical pl-2 pr-2'></i></a>")
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

function attachListToTableFromGeneralSearch(response, headerTitle) {
    $("#lblTblHeaderPlaylistName")
        .attr("data-playlistid", "")
        .html(headerTitle || "Search Results");

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
                    $("<a class=\"font-sz-11pt btn-inner-more-none-list-options\"><i class='fa-solid fa-ellipsis-vertical pl-2 pr-2'></i></a>")
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
                initializeHomeRadio(videoId);

                armaradio.masterPageWait(false);
            } else {
                armaradio.masterAJAXPost({
                    artistName: artistName,
                    songName: songName
                }, "Music", "GetUrlByArtistSongName")
                    .then(function (response) {
                        if (response) {
                            if (response.hasVideo) {
                                currentRow.attr({
                                    "data-videoid": response.videoId,
                                    "data-alternateids": (response.alternateIds || []).join(",")
                                }); //videoId 

                                initializeHomeRadio(response.videoId);

                                armaradio.masterPageWait(false);
                            } else {
                                let newIframe = $("<div></div");
                                newIframe.attr({
                                    "id": "armaMainPlayer",
                                    "class": "iframe-holder"
                                });

                                newIframe.append($("<span class=\"lbl-not-found\"></span>").html("Song not found"));

                                if (localHomePlayer) {
                                    try {
                                        localHomePlayer.dispose();
                                        localHomePlayer = null;
                                    } catch (ex) {

                                    }
                                }

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
                let videoId = $.trim(currentRow.attr("data-videoid"));
                let artistName = $.trim(currentRow.attr("data-artist"));
                let songName = $.trim(currentRow.attr("data-song"));
                let separator = (artistName != "" && songName != "" ? " - " : "");

                if (songId != "") {
                    $("#btnSongOptions_RemoveFromPlaylist").attr("data-id", songId);
                    $("#btnSongOptions_RemoveFromPlaylist").attr("data-artist", artistName);
                    $("#btnSongOptions_RemoveFromPlaylist").attr("data-song", songName);
                    $("#lblSongOptionsTitle").html(artistName + separator + songName);

                    $(".btn-options-download").attr({
                        "data-artist": artistName,
                        "data-song": songName,
                        "data-videoid": videoId
                    });

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

                $(".btn-options-download").attr({
                    "data-artist": artistName,
                    "data-song": songName,
                    "data-videoid": videoId
                });

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

function initializeHomeRadio(videoId, disponse) {
    if (!localHomePlayer || disponse) {
        let newIframe = $("<video></video");
        newIframe.attr({
            "id": "armaMainPlayer",
            "class": "iframe-holder video-js vjs-default-skin"
        });

        if (localHomePlayer) {
            try {
                localHomePlayer.dispose();
                localHomePlayer = null;
            } catch (ex) {

            }
        }

        $("#dvMainPlay_currentlyPlaying").find(".iframe-holder").remove();
        $("#dvMainPlay_currentlyPlaying").append(newIframe);

        localHomePlayer = videojs("armaMainPlayer", {
            width: 356,
            height: 200,
            autoplay: true,
            controls: true,
            poster: "https://random-image-pepebigotes.vercel.app/api/random-image?g=" + generateGUID(),
            sources: [{
                src: (ajaxPointCall + "/Music/FetchAudioFile?VideoId=" + videoId),
                type: "audio/webm"
            }]
        });
        localHomePlayer.soundWave({
            waveColor: soundWaveColor,
            waveWidth: 356,
            waveHeight: 200
        });

        localHomePlayer.on("error", function () {
            onPlayerError(localHomePlayer.error());
        });
        localHomePlayer.on("ready", function () {
            restoreVolume(localHomePlayer);
            localHomePlayer.play();
        });
        localHomePlayer.on("ended", function () {
            onPlayerStateChange();
        });
        localHomePlayer.on("previous", function () {
            localHomePlayer.stop();
            localHomePlayer.currentTime(0);
            localHomePlayer.play();
        });
        localHomePlayer.on("next", function () {
            playerPlayNext();
        });
        localHomePlayer.on("volumechange", function () {
            saveVolume(localHomePlayer);
        });
    } else {
        let newPoster = "https://random-image-pepebigotes.vercel.app/api/random-image?g=" + generateGUID();
        let newSource = ajaxPointCall + "/Music/FetchAudioFile?VideoId=" + videoId;

        localHomePlayer.poster(newPoster);        
        localHomePlayer.src({
            type: "audio/webm",
            src: newSource
        });
        
        localHomePlayer.load();
        localHomePlayer.play();
    }
}

function saveVolume(currentPlayer) {
    localStorage.setItem("videoVolume", currentPlayer.volume());
}

function restoreVolume(currentPlayer) {
    let savedVolume = localStorage.getItem("videoVolume");
    if (savedVolume !== null) {
        currentPlayer.volume(parseFloat(savedVolume));
    }
}

function onPlayerReady(e) {
    e.target.playVideo();
}

//function onPlayerStateChange(e) {
//    if (e.data == YT.PlayerState.ENDED) {
//        let currentStatus = $.trim($("#btnMainPlayerToggleRepeat").attr("data-status"));

//        if (currentStatus == "1") {
//            e.target.playVideo();
//        } else {
//            playerPlayNext();
//        }
//    } else {
//        console.log(e);
//    }
//}

function onPlayerStateChange() {
    let currentStatus = $.trim($("#btnMainPlayerToggleRepeat").attr("data-status"));

    if (currentStatus == "1") {
        localHomePlayer.currentTime(0);
        localHomePlayer.play();
    } else {
        playerPlayNext();
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
            getAlternateId(lastPlayedRow);

            if ($.trim(lastPlayedRow.attr("data-videoid")) != "") {
                nextPlay = lastPlayedRow;
            } else {
                let indexRow = lastPlayedRow.index() + 1;
                nextPlay = $("#tblMainPlayList").find("tr").eq(indexRow);
            }
        } else {
            let indexRow = lastPlayedRow.index() + 1;
            nextPlay = $("#tblMainPlayList").find("tr").eq(indexRow);
        }

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

function getAlternateId(tr) {
    let allIds = ($.trim(tr.attr("data-alternateids")) == "" ? [] : ($.trim(tr.attr("data-alternateids"))).split(","));

    if (allIds.length) {
        tr.attr("data-videoid", allIds[0]);
        allIds.shift();

        tr.attr("data-alternateids", (allIds || []).join(","));
    } else {
        tr.attr("data-videoid", "");
    }
}