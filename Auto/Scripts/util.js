//$(function () {

//    // Ссылка на автоматически-сгенерированный прокси хаба
//    var chat = $.connection.chatHub;
//    // Объявление функции, которая хаб вызывает при получении сообщений
//    chat.client.addMessage = function (name, message) {
//        $('#divmsg').append("<p>" +name+':'+ message + "</p>");
//    };

//    // Функция, вызываемая при подключении нового пользователя
//    chat.client.onConnected = function (id, userName) {
//        $('#hdId').val(id);
//        $('#username').val(userName);

//    }
//    // Открываем соединение
//    $.connection.hub.start().done(function () {

//        var name = $("#username").val();
//        var chatid = $("#SpeachId").val;
//        chat.server.connect(chatid,name);
        

//        $('#btnsend').click(function () {
//            // Вызываем у хаба метод Send
//            chat.server.send($('#username').val(), $('#message').val());
//            $('#message').val('');
//        });
//    });
//});
//// Кодирование тегов
//function htmlEncode(value) {
//    var encodedValue = $('<div />').text(value).html();
//    return encodedValue;
//}


$(function () {

    // $('#chatBody').hide();
    //$('#loginBlock').show();
    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.chatHub;
    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (name, message) {
        // Добавление сообщений на веб-страницу 
        $('#chatroom').append('<p><b>' + htmlEncode(name)
            + '</b>: ' + htmlEncode(message) + '</p>');
    };

    // Функция, вызываемая при подключении нового пользователя
    chat.client.onConnected = function (id, userName) {

        // установка в скрытых полях имени и id текущего пользователя
        $('#hdId').val(id);
        $('#username').val(userName);
                
    }

    // Открываем соединение
    $.connection.hub.start().done(function () {
                
        var flag = false; 

        $('#sendmessage').click(function () {
            if (flag==false) {
                var name = $("#username").val();
                chat.server.connect(name);
                flag = true;
                $('#header').html('<h3>'+flag.value);
            }
            // Вызываем у хаба метод Send
            chat.server.send($('#username').val(), $('#message').val());
            send();
            $('#message').val('');
            
        });

        // обработка логина
        //$("#btnLogin").click(function () {

        //    var name = $("#txtUserName").val();
        //    if (name.length > 0) {
        //        chat.server.connect(name);
        //    }
        //    else {
        //        alert("Введите имя");
        //    }
        //});
    });
});
// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}


function send() {
    var message = $("#message").val();
    var SpeachId = $("#SpeachId").val();
    var name = $("#username").val();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"message":"' + message + '","name":"' + name + '","SpeachId":"' + SpeachId + '"}',
        url: "sendmsg",
        dataType: "json",        
    });
}