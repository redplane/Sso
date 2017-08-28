loginGoogle = function(user) {
	// useful data for your client-side scripts:
	var profile = user.getbasicprofile();
	console.log("id: " + profile.getid()); // don't send this directly to your server!
	console.log('full name: ' + profile.getname());
	console.log('given name: ' + profile.getgivenname());
	console.log('family name: ' + profile.getfamilyname());
	console.log("image url: " + profile.getimageurl());
	console.log("email: " + profile.getemail());

	// the id token you need to pass to your backend:-->
	var id_token = user.getauthresponse().id_token;
	console.log("id token: " + id_token);
};