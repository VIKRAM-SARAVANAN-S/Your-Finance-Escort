# Your-Finance-Escort
This Repository is created for Capstone Project (Financial Management)...

# Personal Finance Management Tool (Microservices Architecture)

This repository outlines a personal finance management tool built using a microservices architecture, addressing the challenges of budgeting, saving, and financial awareness.

## Abstract

This project aims to provide users with a comprehensive and automated financial management experience. Utilizing a microservices approach, the system offers distinct functionalities for authentication, bank transaction processing, automated savings generation, report generation and delivery, and data visualization. This modular design promotes scalability, maintainability, and independent deployment.

## Microservices

* **Authentication & Authorization:**
    * Handles user registration, login, and access control.
    * Ensures secure access to financial data.
* **Bank Transaction (Global API):**
    * Integrates with a global banking API to fetch and process transaction data.
    * Categorizes expenses automatically.
    * Provides a standardized interface for various bank integrations.
* **Savings Generation with Algorithm:**
    * Implements algorithms to analyze spending patterns and generate personalized savings plans.
    * Calculates optimal savings amounts and schedules.
    * Tracks progress towards savings goals.
* **SendGrid API + Twilio (PDF Generation + Email/SMS):**
    * Generates PDF reports of financial summaries and transactions.
    * Utilizes SendGrid for email delivery of reports.
    * Utilizes Twilio for SMS notifications, and alerts.
* **Visualization using Ngx:**
    * Provides a user-friendly interface for visualizing financial data.
    * Uses Ngx (Angular components) to create interactive charts and graphs.
    * Displays spending patterns, budget progress, and savings goals.
