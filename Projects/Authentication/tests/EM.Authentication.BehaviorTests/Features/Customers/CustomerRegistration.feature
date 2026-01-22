Feature: Customer registration

In order to use the platform
As a new customer
I want to create my customer profile

@CustomerRegistration
Scenario: Register a new customer successfully
	Given I have customer registration data with "valid values"
	When I register the customer
	Then The new customer must be registered

@CustomerRegistration
Scenario: Register customer with default values
	Given I have customer registration data with "default values"
	When I register the customer
	Then the customer registration must be rejected due to validation errors
		| message                                   |
		| The username cannot be null or empty      |
		| The email address cannot be null or empty |
		| The password cannot be null or empty      |

@CustomerRegistration
Scenario: Register customer with null values
	Given I have customer registration data with "null values"
	When I register the customer
	Then the customer registration must be rejected due to validation errors
		| message                                   |
		| The username cannot be null or empty      |
		| The email address cannot be null or empty |
		| The password cannot be null or empty      |

@CustomerRegistration
Scenario: Register customer with username and email address greater than max lenght
	Given I have customer registration data with "values greater than max lenght"
	When I register the customer
	Then the customer registration must be rejected due to validation errors
		| message                                      |
		| The username cannot be greater than 50       |
		| The email address cannot be greater than 100 |

@CustomerRegistration
Scenario: Register customer with invalid email address and password
	Given I have customer registration data with "invalid email address and password"
	When I register the customer
	Then the customer registration must be rejected due to validation errors
		| message                      |
		| The email address is invalid |
		| The password is invalid      |

@CustomerRegistration
Scenario: Register customer with a different password confirmation
	Given I have customer registration data with "password different"
	When I register the customer
	Then the customer registration must be rejected due to validation errors
		| message                                         |
		| The password is different from the confirmation |
