$(function () {
    var DEFAULT_AJAX_TIMEOUT = 50000;

    function search() {
        resetAllGrid();
        $.ajax({
            type: "POST",
            url: "/Analyser/RegisterSearchValue",
            data: { Value: $('#Value').val() },
            timeout: DEFAULT_AJAX_TIMEOUT,
            success: function (data) {
                if (data) {
                    $("#ContentId").val(data);

                    var isStopWordFilterOn = $("#IsFilterStopWordsOn").is(':checked');
                    
                    if ($("#IsCalculateNumberOfWordOccurencesOn").is(':checked')) {
                        getNumberOfWordOccurancesInPage(data, isStopWordFilterOn);
                    }

                    if ($("#IsCalculateNumberOfOccurencesInMetaTagOn").is(':checked')) {
                        getNumberOfWordOccurancesInMetaTags(data, isStopWordFilterOn);
                    }

                    if ($("#IsCalculateNumberOfExternalLinkOn").is(':checked')) {
                        getNumberOfUrlOccurancesInPage(data);
                    }
                    
                } else {
                    ajaxNoResult("unable to get value.");
                }
            }, error: function (error) {
                ajaxFailed(error);
            }
        });
    }

    function getNumberOfWordOccurancesInPage(contentId, isStopWordFilterOn) {
        var divToappendId = "DivCalculateNumberOfWordOccurencesInPage";
        $('#' + divToappendId).empty();

        if (contentId) {
            var params = {
                contentId: contentId,
                isStopWordFilterOn: isStopWordFilterOn
            };
            
            $.ajax({
                type: "GET",
                url: "/Analyser/GetNumberOfWordOccursOnValue",
                data: params,
                timeout: DEFAULT_AJAX_TIMEOUT,
                success: function (data) {
                    if (data) {
                        renderDataTables(data, "Calculate number of word occurences in page", divToappendId);
                    } else {
                        ajaxNoResult("No result found for word occurences in page.");
                    }
                }, error: function (error) {
                    ajaxFailed(error);
                }
            });
        } 
    }

    function getNumberOfWordOccurancesInMetaTags(contentId, isStopWordFilterOn) {
        var divToappendId = "DivCalculateNumberOfWordOccurencesInMetaTags";
        $('#' + divToappendId).empty();

        if (contentId) {
            var params = {
                contentId: contentId,
                isStopWordFilterOn: isStopWordFilterOn
            };

            $.ajax({
                type: "GET",
                url: "/Analyser/GetNumberOfWordOccursOnMetaTag",
                data: params,
                timeout: DEFAULT_AJAX_TIMEOUT,
                success: function (data) {
                    if (data) {
                        renderDataTables(data, "Calculate number of word occurences in Meta Tags", divToappendId);
                    } else {
                        ajaxNoResult("No result found for word occurences in Meta Tags.");
                    }
                }, error: function (error) {
                    ajaxFailed(error);
                }
            });
        }
    }

    function getNumberOfUrlOccurancesInPage(contentId) {
        var divToappendId = "DivCalculateNumberOfUrlOccurencesInPage";
        $('#' + divToappendId).empty();

        if (contentId) {
            var params = {
                contentId: contentId
            };

            $.ajax({
                type: "GET",
                url: "/Analyser/GetNumberOfExternalUrlOccursOnValue",
                data: params,
                timeout: DEFAULT_AJAX_TIMEOUT,
                success: function (data) {
                    if (data) {
                        renderDataTables(data, "Calculate number of urls occurences in Page", divToappendId);
                    } else {
                        ajaxNoResult("No result found for number of urls occurences in page.");
                    }
                }, error: function (error) {
                    ajaxFailed(error);
                }
            });
        }
    }

    function renderDataTables(data, title, divToAppendId) {
        var div = $('#' + divToAppendId);
        var title = $('<h2>').text(title);
        div.append(title);
        var table = $('<table>').prop("id", "table_" + divToAppendId).addClass("display");
        div.append(table);

        $('#table_' + divToAppendId).DataTable({
            searching: false,
            lengthChange: false,
            data: data,
            columns: [
                { title: "Word", data: 'word' },
                { title: "Frequency", data: 'frequency' }
            ]
        });
    }

    function ajaxFailed(error) {
        alert(error.responseText);
    }

    function ajaxNoResult(errorMsg) {
        alert(errorMsg);
    }

    function resetAllGrid() {
        $('#DivCalculateNumberOfWordOccurencesInPage').empty();
        $('#DivCalculateNumberOfWordOccurencesInMetaTags').empty();
        $('#DivCalculateNumberOfUrlOccurencesInPage').empty();
    }

    $("form").submit(function (e) {
        e.preventDefault();

        if ($("#Value").val().trim().length <= 0) {
            alert("Please key in some text or Url");
            return;
        }

        search(e);
    });

});

