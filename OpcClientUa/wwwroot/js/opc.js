"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/opcHub").build();

connection.on("OpcDataUpdate", function (opcItem) {
    var itemDiv = $('[id="' + opcItem.itemId + '"]');
    //id对应的元素存在，则只更新数值、质量、时间
    //id对应的元素不存在，则新增元素及其自元素
    if (itemDiv.length > 0) {
        itemDiv.find('.opcValue').text(opcItem.value);
        itemDiv.find('.opcQuality').text(opcItem.quality);
        itemDiv.find('.opcTimeStamp').text(opcItem.timeStamp);
    } else {
        itemDiv = $('<div id="' + opcItem.itemId + '"></div>');
        itemDiv.addClass('card').addClass('bg-success').addClass('text-white');
        var cardHeader = $('<h4></h4>');
        cardHeader.addClass('card-header').addClass('opcItemId');
        cardHeader.text(opcItem.itemId);
        itemDiv.append(cardHeader);
        var cardBody = $('<div></div>');
        cardBody.addClass('card-body');
        var cardText = $('<div></div>');
        cardText.addClass('card-text');
        var row = $('<div></div>');
        row.addClass('row');
        var opcValue = $('<h5></h5>');
        opcValue.addClass('col-6').addClass('opcValue');
        opcValue.text(opcItem.value);
        var opcQuality = $('<h5></h5>');
        opcQuality.addClass('col-6').addClass('opcQuality');
        opcQuality.text(opcItem.quality);
        row.append(opcValue);
        row.append(opcQuality);
        var div = $('<div></div>');
        var opcTimeStamp = $('<h5></h5>');
        opcTimeStamp.addClass('opcTimeStamp');
        opcTimeStamp.text(opcItem.timeStamp);
        div.append(opcTimeStamp);
        cardText.append(row);
        cardText.append(div);
        cardBody.append(cardText);
        itemDiv.append(cardBody);
        $('.card-deck').append(itemDiv);
    }
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});