module.exports = function(app, passport) {

  app.get('/', function(req, res) {
    res.render('index.ejs', { message: req.flash('loginMessage') });
  });

  app.post('/', passport.authenticate('local-login', {
    successRedirect: '/database',
    failureRedirect: '/',
    failureFlash: true
  }));

  app.get('/signup', function(req, res) {
    res.render('signup.ejs', { message: req.flash('signupMessage') });
  });

  app.post('/signup', passport.authenticate('local-signup', {
    successRedirect: '/',
    failureRedirect: '/signup',
    failureFlash: true
  }));

  app.get('/database', isLoggedIn, function(req, res) {
    res.render('database.ejs', {
      user: req.user
    });
  });

  app.get('/logout', function(req, res) {
    req.logout();
    res.redirect('/');
  });

}

function isLoggedIn(req, res, next) {

  if (req.isAuthenticated())
    return next();

  res.redirect('/');
}
