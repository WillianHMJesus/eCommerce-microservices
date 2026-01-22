Feature: User management

In order to manage platform users
As an authorized user
I want to create and maintain user accounts

@UserRegistration
Scenario: Register a new user successfully
	Given I am authenticated in with the "manager profile"
	And I have user registration data with "valid values"
	When I register the user
	Then the new user must be registered

@UserRegistration
Scenario: Register a user with an unauthorized profile
	Given I am authenticated in with the "customer profile"
	And I have user registration data with "valid values"
	When I register the user
	Then the user registration must be rejected due to insufficient permissions

@UserRegistration
Scenario: Register a user without being authenticated
	Given I am not authenticated
	And I have user registration data with "valid values"
	When I register the user
	Then the user registration must be rejected because the user is not authenticated

@UserRegistration
Scenario: Register a user with invalid authenticated
	Given I am not authenticated due to invalid credentials
	And I have user registration data with "valid values"
	When I register the user
	Then the user registration must be rejected because the user is not authenticated

@UserRegistration
Scenario: Register user with default values
	Given I am authenticated in with the "manager profile"
	And I have user registration data with "default values"
	When I register the user
	Then the user registration must be rejected due to validation errors
		| message                                   |
		| The username cannot be null or empty      |
		| The email address cannot be null or empty |
		| The password cannot be null or empty      |

@UserRegistration
Scenario: Register user with null values
	Given I am authenticated in with the "manager profile"
	And I have user registration data with "null values"
	When I register the user
	Then the user registration must be rejected due to validation errors
		| message                                   |
		| The username cannot be null or empty      |
		| The email address cannot be null or empty |
		| The password cannot be null or empty      |

@UserRegistration
Scenario: Register user with username and email address greater than max lenght
	Given I am authenticated in with the "manager profile"
	And I have user registration data with "values greater than max lenght"
	When I register the user
	Then the user registration must be rejected due to validation errors
		| message                                      |
		| The username cannot be greater than 50       |
		| The email address cannot be greater than 100 |

@UserRegistration
Scenario: Register user with invalid email address and password
	Given I am authenticated in with the "manager profile"
	And I have user registration data with "invalid email address and password"
	When I register the user
	Then the user registration must be rejected due to validation errors
		| message                      |
		| The email address is invalid |
		| The password is invalid      |

@UserRegistration
Scenario: Register user with a different password confirmation
	Given I am authenticated in with the "manager profile"
	And I have user registration data with "password different"
	When I register the user
	Then the user registration must be rejected due to validation errors
		| message                                         |
		| The password is different from the confirmation |

@UserRegistration
Scenario: Register user with a profile name not found
	Given I am authenticated in with the "manager profile"
	And I have user registration data with "profile not found"
	When I register the user
	Then the user registration must be rejected due to validation errors
		| message                    |
		| The user profile not found |

@UserRegistration
Scenario: Change the password of a user logged in with a manager profile
	Given I am authenticated in with the "manager profile"
	And I have password change data for my user with "manager profile"
	When I change my password
	Then the password must be changed

@UserPasswordChange
Scenario: Change the password of a user logged in with a customer profile
	Given I am authenticated in with the "customer profile"
	And I have password change data for my user with "customer profile"
	When I change my password
	Then the password must be changed

@UserPasswordChange
Scenario: Change another user's password while authenticated with the manager profile
	Given I am authenticated in with the "manager profile"
	And I have password change data for another user with "customer profile"
	When I change my password
	Then the password must be changed

@UserPasswordChange
Scenario: Change another user's password while authenticated with the customer profile
	Given I am authenticated in with the "customer profile"
	And I have password change data for another user with "manager profile"
	When I change my password
	Then the password must not be changed due to insufficient permissions

@UserPasswordChange
Scenario: Change user password without being authenticated
	Given I am not authenticated
	And I have password change data for user with "valid values"
	When I change my password
	Then the password change must be rejected because the user is not authenticated

@UserPasswordChange
Scenario: Change user password with invalid authenticated
	Given I am not authenticated due to invalid credentials
	And I have password change data for user with "valid values"
	When I change my password
	Then the password change must be rejected because the user is not authenticated
	
@UserPasswordChange
Scenario: Change user password with default values
	Given I am authenticated in with the "manager profile"
	And I have password change data for user with "default values"
	When I change my password
	Then the password must not be changed due to validation errors
		| message                                   |
		| The email address cannot be null or empty |
		| The password cannot be null or empty      |

@UserPasswordChange
Scenario: Change user password with null values
	Given I am authenticated in with the "manager profile"
	And I have password change data for user with "null values"
	When I change my password
	Then the password must not be changed due to validation errors
		| message                                   |
		| The email address cannot be null or empty |
		| The password cannot be null or empty      |

@UserPasswordChange
Scenario: Change user password with email address greater than max lenght
	Given I am authenticated in with the "manager profile"
	And I have password change data for user with "email address greater than max lenght"
	When I change my password
	Then the password must not be changed due to validation errors
		| message                                      |
		| The email address cannot be greater than 100 |

@UserPasswordChange
Scenario: Change user password with invalid email address and password
	Given I am authenticated in with the "manager profile"
	And I have password change data for user with "invalid email address and password"
	When I change my password
	Then the password must not be changed due to validation errors
		| message                      |
		| The email address is invalid |
		| The password is invalid      |

@UserPasswordChange
Scenario: Change user password with a different password confirmation
	Given I am authenticated in with the "manager profile"
	And I have password change data for user with "password different"
	When I change my password
	Then the password must not be changed due to validation errors
		| message                                         |
		| The password is different from the confirmation |

@UserPasswordChange
Scenario: Change user password with a user not found
	Given I am authenticated in with the "manager profile"
	And I have password change data for user with "user not found"
	When I change my password
	Then the password must not be changed due to validation errors
		| message                                    |
		| The email address or password is incorrect |

@UserPasswordChange
Scenario: Change user password with incorrect password
	Given I am authenticated in with the "manager profile"
	And I have password change data for user with "incorrect password"
	When I change my password
	Then the password must not be changed due to validation errors
		| message                                    |
		| The email address or password is incorrect |
