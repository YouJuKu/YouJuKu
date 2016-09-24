var schedulerClient = {
    init: function () {
        scheduler.serverList("blocked_time");//initialize server list before scheduler initialization
 
        scheduler.attachEvent("onXLS", function () {
            scheduler.config.readonly = true;
        });
 
        scheduler.attachEvent("onXLE", function () {
            var blocked = scheduler.serverList("blocked_time");
            schedulerClient.updateTimespans(blocked);
            blocked.splice(0, blocked.length);
 
            //make scheduler editable again and redraw it to display loaded timespans
            scheduler.config.readonly = false;
            scheduler.setCurrentView();
        });
    },
     
    updateTimespans: function (timespans) {
        // preprocess loaded timespans and add them to the scheduler
        for (var i = 0; i < timespans.length; i++) {
            var span = timespans[i];
             
            span.start_date = scheduler.templates.xml_date(span.StartDate);
            span.end_date = scheduler.templates.xml_date(span.EndDate);
 
            // add a label
            span.html = scheduler.templates.event_date(span.start_date) +
                " - " +
                scheduler.templates.event_date(span.end_date);
 
 
            //define timespan as 'blocked'
            span.type = "dhx_time_block";
            scheduler.deleteMarkedTimespan(span);// prevent overlapping
 
            scheduler.addMarkedTimespan(span);
        }
    }
};

var schedulerAdmin = {
    init: function () {

        scheduler.templates.event_bar_text = scheduler.templates.event_text =
            function (start, end, ev) {
                var user = schedulerAdmin.findUser(ev.UserId);
                var text = (user ? "<b>" + user.label + "</b> - " : "") + ev.text;

                return text;
            }

    },

    findUser: function (id) {
        var users = scheduler.serverList("users");
        for (var i = 0; i < users.length; i++) {
            if (users[i].key == id)
                return users[i];
        }
    }
};