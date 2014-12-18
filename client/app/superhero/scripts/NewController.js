angular
  .module('superhero')
  .controller("NewController", function ($scope, Superhero, supersonic) {
    $scope.superhero = {};

    $scope.submitForm = function () {
      $scope.showSpinner = true;
      newsuperhero = new Superhero($scope.superhero);
      newsuperhero.save().then( function () {
        supersonic.ui.modal.hide();
      });
    };

    $scope.cancel = function () {
      supersonic.ui.modal.hide();
    }

  });