"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/opcHub").build();

connection.on("OpcDataUpdate", function (opcItem) {
    /**
     * 百度语音
     * */
    //var url = 'http://tts.baidu.com/text2audio?lan=zh&ie=UTF-8&per=3&spd=5&text=123';
    //var audio = new Audio();
    //audio.src = url;
    //audio.play();

    /**
     * html语音
     * */
    var itemDiv = $('[id="' + opcItem.itemId + '"]');
    var value = Number(opcItem.value);
    if (itemDiv.find('.opcValue').text() != opcItem.value &&
        ((null != opcItem.highAlarm && value > opcItem.highAlarm)
            || (null != opcItem.highAlarm && value < opcItem.lowAlarm))
    ) {
        speechSynthesis.speak(new SpeechSynthesisUtterance(opcItem.itemId + "报警，报警值为" + opcItem.value));
    }
    //id对应的元素存在，则只更新数值、质量、时间
    //id对应的元素不存在，则新增元素及其自元素
    if (itemDiv.length > 0) {
        itemDiv.find('.opcValue').text(opcItem.value);
        itemDiv.find('.opcQuality').text(opcItem.quality);
        itemDiv.find('.opcTimeStamp').text(opcItem.timeStamp);
    } else {
        var opcValue = $('<h5></h5>')
            .addClass('col-6 opcValue')
            .text(opcItem.value);
        var opcQuality = $('<h5></h5>')
            .addClass('col-6 opcQuality')
            .text(opcItem.quality);
        var row = $('<div></div>')
            .addClass('row')
            .append(opcValue, opcQuality);

        var opcTimeStamp = $('<h5></h5>')
            .addClass('opcTimeStamp')
            .text(opcItem.timeStamp);
        var div = $('<div></div>')
            .append(opcTimeStamp);

        var cardText = $('<div></div>')
            .addClass('card-text')
            .append(row, div);

        var cardBody = $('<div></div>')
            .addClass('card-body')
            .append(cardText);

        var cardHeader = $('<h4></h4>')
            .addClass('card-header opcItemId')
            .text(opcItem.itemId);

        itemDiv = $('<div></div>')
            .attr('id', opcItem.itemId)
            .addClass('card bg-success text-white')
            .append(cardHeader, cardBody);

        $('.card-deck').append(itemDiv);
    }
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});