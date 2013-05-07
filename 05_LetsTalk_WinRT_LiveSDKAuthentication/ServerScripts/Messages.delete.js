function del(id, user, request) {
	var messagesTable = tables.getTable('messages');

    messagesTable.where({
        id: id,
        userId: user.userId
    }).read({
        success: function(results) {
        	if (results.length === 0) {
            	request.respond(statusCodes.UNAUTHORIZED, 'You can only delete you own messages.');
            }
            else {
                request.execute();
            }
        }
    });
}