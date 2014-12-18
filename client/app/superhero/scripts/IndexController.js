angular
  .module('superhero')
  .controller("IndexController", function ($scope, Superhero, supersonic) {
    $scope.superheros = null;
    $scope.showSpinner = true;

    Superhero.all().whenChanged( function (superheros) {
        $scope.$apply( function () {
          $scope.superheros = superheros;
          $scope.showSpinner = false;
        });
    });

  });