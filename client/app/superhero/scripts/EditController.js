angular
  .module('superhero')
  .controller("EditController", function ($scope, Superhero, supersonic) {
    $scope.superhero = null;
    $scope.showSpinner = true;

    // Fetch an object based on id from the database
    Superhero.find(steroids.view.params.id).then( function (superhero) {
      $scope.$apply(function() {
        $scope.superhero = superhero;
        $scope.showSpinner = false;
      });
    });

    $scope.submitForm = function() {
      $scope.showSpinner = true;
      $scope.superhero.save().then( function () {
        supersonic.ui.modal.hide();
      });
    }

    $scope.cancel = function () {
      supersonic.ui.modal.hide();
    }

  });
