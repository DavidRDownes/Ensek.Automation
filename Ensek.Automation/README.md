# ENSEK.Automation Technical Test

## Solution Overview

API Testing solution using Specflow, NUnit, HttpClient

## Future Development

- Seprate Feature Files, Step Definitions and Helpers into seperate projects. This will help long term maintainability
- Increase test coverage
- Additonal tests around API contracts (currently responses are inconsistent/incorrect)
- Domain level testing 
- Code quality
- Better namespace breakdown (Make it more domain specifc)

## Other Considerations

- Explore other Http client libraries (Ease of development)
    - [RestSharp](https://restsharp.dev/)
    - [Flurl](https://flurl.dev/docs/fluent-http/)
- Could step defintions be made cleaner with abstraction