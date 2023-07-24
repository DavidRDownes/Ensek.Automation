Feature: Orders

Tests against the order API

Scenario: Verify previous orders
	Given I send valid credentials to the Login endpoint
	| Username | Password |
	| test     | testing  |
	When I make a POST request to the reset endpoint

	When I call the orders endpoint
	Then I verify there are 5 previous orders

Scenario: Check for electricity order
	Given I buy 5 units of energy type 3
	When I call the orders endpoint
	Then the order is present

Scenario: Check for oil order
	Given I buy 5 units of energy type 4
	When I call the orders endpoint
	Then the order is present

Scenario: Check for nuclear order
	Given I buy 5 units of energy type 2
	When I call the orders endpoint
	Then the order is present
	
Scenario: Check for gas order
	Given I buy 5 units of energy type 1
	When I call the orders endpoint
	Then the order is present

Scenario: Order Nuclear
	Given I buy 5 units of energy type 2
	Then My purchase was successful

Scenario: Order Oil
	Given I buy 5 units of energy type 4
	Then My purchase was successful

Scenario: Order Gas
	Given I buy 5 units of energy type 1
	Then My purchase was successful

Scenario: Order Electricity
	Given I buy 5 units of energy type 3
	Then My purchase was successful