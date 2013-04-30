function insert(item, user, request) {
    if (!item.body || item.body.length < 2) {
        request.respond(statusCodes.BAD_REQUEST, 'The message text must be 2 or more characters long.');
        return;
    }

    item.createdAt = new Date();

    request.execute();
}