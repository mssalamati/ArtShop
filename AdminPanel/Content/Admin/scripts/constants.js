$.staticApp = {
  name: 'Reactor',
  author: 'Nyasha',
  version: '1.0.0',
  year: (new Date()).getFullYear(),
  font: 'Tahoma, "Helvetica Neue", Helvetica, sans-serif',
  'default': $('<div>').appendTo('body').addClass('bg-default').css('background-color'),
  primary: $('<div>').appendTo('body').addClass('bg-primary').css('background-color'),
  success: $('<div>').appendTo('body').addClass('bg-success').css('background-color'),
  warning: $('<div>').appendTo('body').addClass('bg-warning').css('background-color'),
  danger: $('<div>').appendTo('body').addClass('bg-danger').css('background-color'),
  info: $('<div>').appendTo('body').addClass('bg-info').css('background-color'),
  white: $('<div>').appendTo('body').addClass('bg-white').css('background-color'),
  dark: $('<div>').appendTo('body').addClass('bg-dark').css('background-color'),
  border: '#e4e4e4',
  bodyBg: $('body').css('background-color'),
  textColor: '#6B6B6B'
};