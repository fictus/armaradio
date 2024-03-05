
function masterFixResponseErrorMessage(errorMsg) {
    let returnItem = "";

    if ($.trim(errorMsg) == "") {
        returnItem = "An error occurred while performing this operation";
    } else if ((errorMsg || "").toLowerCase() == "internal server error") {
        returnItem = "An error occurred while performing this operation";
    } else {
        returnItem = errorMsg;
    }

    return returnItem;
}

function ajaxHandleRedirect(response, fetchData) {
    var d = $.Deferred();

    $.ajax({
        url: response.url,
        data: {}
    })
        .done(function (response) {
            let bodyResponse = $("<div></div>");
            bodyResponse.append(response);

            let postUrl = bodyResponse.find("form").attr("action");
            let postParams = {};

            bodyResponse.find("form").find("input[type='hidden']").each(function () {
                let currentInput = $(this);

                postParams[currentInput.attr("name")] = currentInput.val();
            });

            $.ajax({
                type: "POST",
                url: postUrl,
                data: JSON.stringify(postParams),
                contentType: "application/json; charset=utf-8",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-CSRF": "1"
                }
            })
                .done(function (newResponse) {
                    if (micronet.isValidJsonObject(newResponse)) {
                        fetch(fetchData.url, fetchData.fetchBody)
                            .then(function (refetchResponse) {
                                return refetchResponse.json();
                            })
                            .then(function (jsonData) {
                                d.resolve(jsonData);
                            })
                            .catch(function (error) {
                                d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error.message) });
                            });
                    } else {
                        let bodyResponse_b = $("<div></div>");
                        bodyResponse_b.append(newResponse);

                        let postUrl_b = bodyResponse_b.find("form").attr("action");
                        let postParams_b = {};

                        bodyResponse_b.find("form").find("input[type='hidden']").each(function () {
                            let currentInput = $(this);

                            postParams_b[currentInput.attr("name")] = currentInput.val();
                        });

                        $.ajax({
                            type: "POST",
                            url: postUrl_b,
                            data: JSON.stringify(postParams_b),
                            contentType: "application/json; charset=utf-8",
                            headers: {
                                "Content-Type": "application/json; charset=utf-8",
                                "X-CSRF": "1"
                            }
                        })
                            .done(function (newResponse_last) {
                                fetch(fetchData.url, fetchData.fetchBody)
                                    .then(function (refetchResponse_last) {
                                        return refetchResponse_last.json();
                                    })
                                    .then(function (jsonData_last) {
                                        d.resolve(jsonData_last);
                                    })
                                    .catch(function (error_last) {
                                        d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error_last.message) });
                                    });
                            })
                            .fail(function (jqXHR, textStatus, errorThrown) {
                                d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(jqXHR.responseText), statusCode: jqXHR.status });
                            });
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(jqXHR.responseText), statusCode: jqXHR.status });
                });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(jqXHR.responseText), statusCode: jqXHR.status });
        });

    return d.promise();
}

function isJSON(str) {
    if (typeof (str) !== 'string') {
        return true;
    }

    try {
        JSON.parse(str);
        return true;
    } catch (e) {
        return false;
    }
}

var armaradio = {
    /************************************************
        isValidJsonObject
    ************************************************/
    isValidJsonObject: function (data) {
        try {
            if ((typeof data) !== "object") {
                let jsonObject = $.parseJSON(data);

                return true;
            } else {
                let jsonObject = $.parseJSON(JSON.stringify(data));

                return true;
            }
        } catch (ex) {

        }

        return false;
    },
    /************************************************
        downloadFileViaGet
    ************************************************/
    downloadFileViaGet: function (endPoint, data) {
        let params = [];

        $.each((data || {}), function (key, val) {
            params.push(key + "=" + encodeURIComponent(val));
        });

        window.location = endPoint + (params.length ? "?" : "") + params.join("&");
    },
    /************************************************
        masterAJAXPost
    ************************************************/
    masterAJAXPost: function (data, controller, handler) {
        let d = $.Deferred();

        try {
            let endPointUrl = (ajaxPointCall ? (ajaxPointCall + "/") : "") + controller + "/" + handler;

            // Default options are marked with *
            let fetchBody = {
                method: "POST", // *GET, POST, PUT, DELETE, etc.
                mode: "cors", // no-cors, *cors, same-origin
                cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
                credentials: "same-origin", // include, *same-origin, omit
                headers: {
                    "Content-Type": "application/json",
                    "Accept-Encoding": "gzip, deflate"
                    // 'Content-Type': 'application/x-www-form-urlencoded',
                },
                redirect: "follow", // manual, *follow, error
                referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
                body: JSON.stringify(data || {}), // body data type must match "Content-Type" header
            };

            fetch(endPointUrl, fetchBody)
                .then(function (response) {
                    if (response.redirected) {
                        return ajaxHandleRedirect(response, {
                            url: endPointUrl,
                            fetchBody: fetchBody
                        });
                    } else {
                        if (response.status >= 500) {
                            return response.text();
                        } else {
                            return response.json();
                        }
                    }
                })
                .then(function (jsonData) {
                    if (isJSON(jsonData)) {
                        d.resolve(jsonData);
                    } else {
                        d.resolve({
                            error: true,
                            errorMsg: masterFixResponseErrorMessage(jsonData)
                        });
                    }
                })
                .catch(function (error) {
                    d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error.message) });
                });
        } catch (error) {
            d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error.message) });
        }

        return d.promise();
    },
    /************************************************
        masterAJAXPostByEndPoint
    ************************************************/
    masterAJAXPostByEndPoint: function (data, endPoint) {
        let d = $.Deferred();

        try {
            let endPointUrl = endPoint;

            // Default options are marked with *
            let fetchBody = {
                method: "POST", // *GET, POST, PUT, DELETE, etc.
                mode: "cors", // no-cors, *cors, same-origin
                cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
                credentials: "same-origin", // include, *same-origin, omit
                headers: {
                    "Content-Type": "application/json",
                    "Accept-Encoding": "gzip, deflate"
                    // 'Content-Type': 'application/x-www-form-urlencoded',
                },
                redirect: "follow", // manual, *follow, error
                referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
                body: JSON.stringify(data || {}), // body data type must match "Content-Type" header
            };

            fetch(endPointUrl, fetchBody)
                .then(function (response) {
                    if (response.redirected) {
                        return ajaxHandleRedirect(response, {
                            url: endPointUrl,
                            fetchBody: fetchBody
                        });
                    } else {
                        if (response.status >= 500) {
                            return response.text();
                        } else {
                            return response.json();
                        }
                    }
                })
                .then(function (jsonData) {
                    if (isJSON(jsonData)) {
                        d.resolve(jsonData);
                    } else {
                        d.resolve({
                            error: true,
                            errorMsg: masterFixResponseErrorMessage(jsonData)
                        });
                    }
                })
                .catch(function (error) {
                    d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error.message) });
                });
        } catch (error) {
            d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error.message) });
        }

        return d.promise();
    },
    /************************************************
        masterAJAXGet
    ************************************************/
    masterAJAXGet: function (data, controller, handler) {
        let d = $.Deferred();

        try {
            let uriParameters = "";
            let uriParts = [];

            $.each((data || {}), function (key, val) {
                uriParts.push(key + "=" + encodeURIComponent(val));
            });

            if (uriParts.length) {
                uriParameters = "?" + uriParts.join("&");
            }

            let endPointUrlPre = (ajaxPointCall ? (ajaxPointCall + "/") : "") + controller + "/" + handler;
            let endPointUrl = (ajaxPointCall ? (ajaxPointCall + "/") : "") + controller + "/" + handler + uriParameters;
            let fetchBody = {
                method: "GET", // *GET, POST, PUT, DELETE, etc.
                mode: "cors", // no-cors, *cors, same-origin
                cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
                credentials: "same-origin", // include, *same-origin, omit
                headers: {
                    "Content-Type": "application/json",
                    "Accept-Encoding": "gzip, deflate"
                    // 'Content-Type': 'application/x-www-form-urlencoded',
                },
                redirect: "follow", // manual, *follow, error
                referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
            };

            fetch(endPointUrl, fetchBody)
                .then(function (response) {
                    if (response.redirected) {
                        return ajaxHandleRedirect(response, {
                            url: endPointUrl,
                            fetchBody: fetchBody
                        });
                    } else {
                        if (response.status >= 500) {
                            return response.text();
                        } else {
                            return response.json();
                        }
                    }
                })
                .then(function (jsonData) {
                    if (isJSON(jsonData)) {
                        d.resolve(jsonData);
                    } else {
                        d.resolve({
                            error: true,
                            errorMsg: masterFixResponseErrorMessage(jsonData)
                        });
                    }
                })
                .catch(function (error) {
                    d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error.message) });
                });
        } catch (error) {
            d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error) });
        }

        return d.promise();
    },
    /************************************************
        masterAJAXGetByUrl
    ************************************************/
    masterAJAXGetByUrl: function (url) {
        let d = $.Deferred();

        try {
            let endPointUrlPre = url.split("?")[0];
            let callUrlParameters = url.split("?")[1];
            let data = micronet.queryStringToJSON(callUrlParameters);

            let endPointUrl = url;
            let fetchBody = {
                method: "GET", // *GET, POST, PUT, DELETE, etc.
                mode: "cors", // no-cors, *cors, same-origin
                cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
                credentials: "same-origin", // include, *same-origin, omit
                headers: {
                    "Content-Type": "application/json",
                    "Accept-Encoding": "gzip, deflate"
                    // 'Content-Type': 'application/x-www-form-urlencoded',
                },
                redirect: "follow", // manual, *follow, error
                referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
            };

            fetch(endPointUrl, fetchBody)
                .then(function (response) {
                    if (response.status >= 500) {
                        return response.text();
                    } else {
                        return response.json();
                    }
                })
                .then(function (jsonData) {
                    if (isJSON(jsonData)) {
                        d.resolve(jsonData);
                    } else {
                        d.resolve({
                            error: true,
                            errorMsg: masterFixResponseErrorMessage(jsonData)
                        });
                    }
                })
                .catch(function (error) {
                    d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error.message) });
                });
        } catch (error) {
            d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error) });
        }

        return d.promise();
    },
    /************************************************
        masterAJAXGetByUrlViaXHR
    ************************************************/
    masterAJAXGetByUrlViaXHR: function (url) {
        let d = $.Deferred();

        try {
            $.ajax({
                type: "GET",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    d.resolve(response);
                }
            })
                .done(function (response) {
                    d.resolve(response);
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(jqXHR.responseText), statusCode: jqXHR.status });
                });
        } catch (error) {
            d.resolve({ error: true, errorMsg: masterFixResponseErrorMessage(error) });
        }

        return d.promise();
    },
    /************************************************
        filterAutoCompleteDataSourceDistinct
    ************************************************/
    filterAutoCompleteDataSourceDistinct: function (grid, refreshDataSource, useFilteredView) {
        let d = $.Deferred();
        let gridFilter = grid.dataSource.filter();
        useFilteredView = true;

        grid.element.find(".k-autocomplete").each(function () {
            let inputBox = $(this).find("input");
            let inputElement = inputBox.data("kendoAutoComplete");
            let columnSouce = inputBox.attr("data-text-field");

            if (inputElement) {
                if (refreshDataSource) {
                    let originalDataSource = grid.dataSource;
                    let flatData = _.uniq((useFilteredView ? originalDataSource.view() : originalDataSource.data()), x => x[columnSouce]);

                    setTimeout(function () {
                        inputElement.dataSource.data(flatData || []);
                    }, 1);
                }

                $.when(inputElement.dataSource.filter(gridFilter))
                    .then(function () {
                        d.resolve();
                    });
            }
        });

        return d.promise();
    },
    /************************************************
        distance
    ************************************************/
    distanceBetweenCoordinates: function (lat1, lon1, lat2, lon2, unit) {
        if ((lat1 == lat2) && (lon1 == lon2)) {
            return 0;
        } else {
            let radlat1 = Math.PI * lat1 / 180;
            let radlat2 = Math.PI * lat2 / 180;
            let theta = lon1 - lon2;
            let radtheta = Math.PI * theta / 180;
            let dist = Math.sin(radlat1) * Math.sin(radlat2) + Math.cos(radlat1) * Math.cos(radlat2) * Math.cos(radtheta);

            if (dist > 1) {
                dist = 1;
            }

            dist = Math.acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            // if (lon1 < 0) {
            //     dist = -dist;
            // }              

            if (unit == "km") {
                dist = dist * 1.609344
            } else if (unit == "N") {
                dist = dist * 0.8684
            }

            return dist;
        }
    },
    /************************************************
        whenAll
        whenAll.apply($, arrDef)
    ************************************************/
    whenAll: function (firstParam) {
        var args = arguments,
            sliceDeferred = [].slice,
            i = 0,
            length = args.length,
            count = length,
            rejected,
            deferred = length <= 1 && firstParam && jQuery.isFunction(firstParam.promise)
                ? firstParam
                : jQuery.Deferred();

        function resolveFunc(i, reject) {
            return function (value) {
                rejected |= reject;
                args[i] = arguments.length > 1 ? sliceDeferred.call(arguments, 0) : value;
                if (!(--count)) {
                    // Strange bug in FF4:
                    // Values changed onto the arguments object sometimes end up as undefined values
                    // outside the $.when method. Cloning the object into a fresh array solves the issue
                    var fn = rejected ? deferred.rejectWith : deferred.resolveWith;
                    fn.call(deferred, deferred, sliceDeferred.call(args, 0));
                }
            };
        }

        if (length > 1) {
            for (; i < length; i++) {
                if (args[i] && jQuery.isFunction(args[i].promise)) {
                    args[i].promise().then(resolveFunc(i), resolveFunc(i, true));
                } else {
                    --count;
                }
            }
            if (!count) {
                deferred.resolveWith(deferred, args);
            }
        } else if (deferred !== firstParam) {
            deferred.resolveWith(deferred, length ? [firstParam] : []);
        }

        return deferred.promise();
    },
    /************************************************
        serializeObjectToFormData
    ************************************************/
    serializeObjectToFormData: function (objData) {
        if (objData) {
            let formData = new FormData();

            for (let key in objData) {
                if (objData.hasOwnProperty(key)) {
                    const value = objData[key];

                    if (Array.isArray(value)) {
                        let rowIndex = 0;

                        value.forEach(item => {
                            for (let itemKey in item) {
                                let key_name = key + "[" + rowIndex + "][" + itemKey + "]";

                                formData.set(key_name, item[itemKey]);
                            }

                            rowIndex++;
                        });
                    } else {
                        formData.set(key, value);
                    }
                }
            }

            return formData;
        }

        return formData;
    },
    /************************************************
        serializeObjectToFormBody
    ************************************************/
    serializeObjectToFormBody: function (objData) {
        let formData = [];

        if (objData) {
            for (let key in objData) {
                if (objData.hasOwnProperty(key)) {
                    const value = objData[key];

                    if (Array.isArray(value)) {
                        let rowIndex = 0;

                        value.forEach(item => {
                            for (let itemKey in item) {
                                let key_name = key + "[" + rowIndex + "][" + itemKey + "]";

                                formData.push($("<input>").attr({
                                    type: "hidden",
                                    name: key_name,
                                    value: item[itemKey]
                                }));
                            }

                            rowIndex++;
                        });
                    } else {
                        formData.push($("<input>").attr({
                            type: "hidden",
                            name: key,
                            value: value
                        }));
                    }
                }
            }
        }

        return formData;
    },
    /************************************************
        redirectViaPost
    ************************************************/
    redirectViaPost: function (endPoint, jsonData, openInNewTab) {
        let postform = $("<form>", {
            action: endPoint,
            method: "post",
            target: openInNewTab ? "_blank" : "_self"
        });
        let formData = micronet.serializeObjectToFormBody(jsonData);

        $.each(formData, function (indx, element) {
            postform.append(element);
        });

        //console.log(postform[0].outerHTML);

        postform.appendTo("body").submit();
        postform.remove();
    },
    /************************************************
        getFileAsBlob
    ************************************************/
    getFileAsBlob: function (endPoint, application_type) {
        return fetch(endPoint)
            .then(function (response) {
                return response.arrayBuffer();
            }).then(function (buffer) {
                let blob = new Blob([buffer], {
                    type: (application_type || "")
                });

                return blob;
            });
    },
    /************************************************
        getFileAsBlob
    ************************************************/
    queryStringToJSON: function (queryString) {
        if (queryString && ($.trim(queryString) != "")) {
            const keyValuePairs = queryString.split("&");

            // Decode the key-value pairs.
            const decodedKeyValuePairs = keyValuePairs.map((keyValuePair) => {
                const [key, value] = keyValuePair.split('=');
                return {
                    key: decodeURIComponent(key),
                    value: decodeURIComponent(value),
                };
            });

            // Create a JSON object from the key-value pairs.
            const jsonObject = {};
            decodedKeyValuePairs.forEach((keyValuePair) => {
                const { key, value } = keyValuePair;
                jsonObject[key] = value;
            });

            return jsonObject;
        } else {
            return {};
        }
    }
};