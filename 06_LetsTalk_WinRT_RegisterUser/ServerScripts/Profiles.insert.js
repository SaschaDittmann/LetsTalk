function insert(item, user, request) {
    var profiles = tables.getTable('profiles');

    if (item.userId !== user.userId) {
        request.respond(statusCodes.BAD_REQUEST, 'A user can only insert a profile for their own userId.');
        return;
    }

    if (!item.name || item.name.length === 0) {
        request.respond(statusCodes.BAD_REQUEST, 'A name must be provided');
        return;
    }

    if (!item.email || item.email.length === 0) {
        request.respond(statusCodes.BAD_REQUEST, 'An email address must be provided');
        return;
    }

    // Check if a user with the same userId already exists
    profiles.where({ userId: item.userId }).read({
        success: function (results) {
            if (results.length > 0) {
                request.respond(statusCodes.BAD_REQUEST, 'Profile already exists.');
                return;
            }

            // No such user exists, add a timestamp and process the insert
            item.memberSince = new Date();
            request.execute();
        }
    });
}