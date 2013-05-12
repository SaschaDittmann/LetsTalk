var profiles = tables.getTable('profiles');

function insert(item, user, context) {
    if (!item.name && item.name.length === 0) {
        context.respond(statusCodes.BAD_REQUEST, 'A name must be provided');
        return;
    }

    if (!item.email && item.email.length === 0) {
        context.respond(statusCodes.BAD_REQUEST, 'An email address must be provided');
        return;
    }

    if (item.userId !== user.userId) {
        context.respond(statusCodes.BAD_REQUEST, 'A user can only insert a profile for their own userId.');
        return;
    }

    // Check if a user with the same userId already exists
    profiles.where({ userId: item.userId }).read({
        success: function (results) {
            if (results.length > 0) {
                context.respond(statusCodes.BAD_REQUEST, 'Profile already exists.');
                return;
            }

            // No such user exists, add a timestamp and proces the insert
            item.memberSince = new Date();
            context.execute();
        }
    });
}