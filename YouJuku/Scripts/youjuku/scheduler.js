var youjukuScheduler = {
    init: function () {
        scheduler.templates.event_bar_text = scheduler.templates.event_text =
            function(start, end, ev) {
                var user = youjukuScheduler.findUser(ev.user_id);
                var text = (user ? "<b>" + user.label + "</b> - " : "") + ev.text;
                ev.color = user.color ? user.color : "";
                return text;
            };
    },
    findUser: function (id)
    {
        var users = scheduler.serverList("users");
        for (var i = 0; i < users.length; i++)
        {
            if (users[i].key === id)
                return users[i];
        }
    }
};

