Feature: Reset

Tests against the reset endpoint

Scenario: Reset Test data successfully
	Given I send valid credentials to the Login endpoint
	| Username | Password |
	| test     | testing  |
	When I make a POST request to the reset endpoint
	Then I get a 200 response
	Then I get a response message