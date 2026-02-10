# AspireTodo
Based on a pronpt to create a demo domain to illustrate some development principles, the response below can serve as a roadmap for building this demo app.

## Sqlite setup
dotnet ef database update 

This should create a ticket.db file in the API project area.  Obv, not a production DB or location, but good enough for development and testing purposes.


A strong, realistic domain for this is an internal **service request & incident management** system (think lightweight IT/help desk) with SLAs, approvals, and escalations. [issuetrak](https://www.issuetrak.com/blog/workflow-examples-choose-the-best-workflow-system)

## Why this domain works

- Familiar mental model: “tickets,” “requests,” “incidents,” and “tasks” exist in almost every enterprise, not just IT. [atlassian](https://www.atlassian.com/agile/project-management/workflow-examples)
- Rich workflows: request intake, triage, assignment, fulfilment, resolution, and closure, with branching for urgent incidents, compliance-related items, and multi-step approvals. [synergycodes](https://www.synergycodes.com/blog/10-workflow-examples-and-use-cases)
- Rule-heavy: SLAs, priority rules, escalation rules, routing rules, and approval matrices all need dynamic evaluation and change over time. [ibm](https://www.ibm.com/docs/en/control-desk/7.6.1?topic=slao-escalations-service-level-agreements)

## Core domain concepts

- Entities: Request (or Ticket), Task, SLA, Escalation, Approval, User, Team, Asset/Service, Knowledge Article. [issuetrak](https://www.issuetrak.com/blog/workflow-examples-choose-the-best-workflow-system)
- States: New → In Triage → In Progress → Waiting on Customer → Resolved → Closed, plus “On Hold” and “Cancelled” variants. [wrike](https://www.wrike.com/blog/workflow-examples/)
- Task types: Incident resolution, service fulfillment (e.g., “new laptop” request), change implementation, review tasks, and approval tasks. [synergycodes](https://www.synergycodes.com/blog/10-workflow-examples-and-use-cases)

One concrete example: “Employee requests VPN access” creates a request, spawns an approval task (manager), then a technical task (IT), both under an SLA with response and resolution targets. [trailhead.salesforce](https://trailhead.salesforce.com/content/learn/modules/approval-process-for-public-sector-solutions/dive-into-approval-workflows)

## Workflows you can model

- Request intake workflow: Form-based submission, auto-categorization, and initial priority based on impact/urgency rules. [atlassian](https://www.atlassian.com/agile/project-management/workflow-examples)
- SLA tracking workflow: Start SLA timers, track first-response and resolution times, and emit events when thresholds are approaching or breached. [manageengine](https://www.manageengine.com/products/service-desk-msp/help/adminguide/configurations/helpdesk/service-catalog/service-sla.html)
- Escalation workflow: If a task is still “In Progress” near SLA breach, escalate to a higher-tier group or manager and increase priority. [purplegriffon](https://purplegriffon.com/blog/what-are-service-desk-slas)
- Approval workflow: Multi-step approvals for specific categories (e.g., software over a cost threshold), with parallel or sequential approval tasks. [applications-platform](https://www.applications-platform.com/template-apps/approval-workflow-template/)

These map nicely onto a state machine plus rule-driven transitions, which is ideal for illustrating workflow and rules engines. [servicenow](https://www.servicenow.com/docs/r/yokohama/build-workflows/approvals/c_ApprovalEngines.html?contentId=~v6vntRVPvx8y~HmAmGP4g)

## Rule evaluation opportunities

- Routing rules: “If category = Network and priority ≥ High, route to Network Ops team; else route to Service Desk.” [issuetrak](https://www.issuetrak.com/blog/workflow-examples-choose-the-best-workflow-system)
- SLA rules: Different SLAs by priority, request type, or customer tier (e.g., VIP users get stricter response times). [ibm](https://www.ibm.com/docs/en/control-desk/7.6.1?topic=slao-escalations-service-level-agreements)
- Escalation rules: Trigger escalation events at N minutes before or after SLA breach; send notifications, reassign, or bump priority. [manageengine](https://www.manageengine.com/products/service-desk-msp/help/adminguide/configurations/helpdesk/service-catalog/service-sla.html)
- Approval rules: Use matrices like “If cost > X and application is ‘Finance system’, require Finance + Security approvals.” [spendflo](https://www.spendflo.com/blog/approval-workflows)

These rules can be modeled as data-driven policies or a rules engine, making it easy to demo dynamic business decisions and configuration-driven behavior. [trailhead.salesforce](https://trailhead.salesforce.com/content/learn/modules/approval-process-for-public-sector-solutions/dive-into-approval-workflows)

## How it can showcase your principles

You can treat “Requests” and “Tasks” as your core aggregates, then use:

- Commands: SubmitRequest, ApproveTask, StartTask, CompleteTask, ReassignTask, ConfigureSla, ConfigureRuleSet, etc. [servicenow](https://www.servicenow.com/docs/r/yokohama/build-workflows/approvals/c_ApprovalEngines.html?contentId=~v6vntRVPvx8y~HmAmGP4g)
- Events: RequestSubmitted, RequestCategorized, SlaApplied, SlaBreached, TaskEscalated, ApprovalGranted, ApprovalRejected. [purplegriffon](https://purplegriffon.com/blog/what-are-service-desk-slas)
- Queries: GetRequestDetails, ListMyTasks, GetSlaStatus, GetPendingApprovals, GetBreachRiskDashboard. [wrike](https://www.wrike.com/blog/workflow-examples/)

That gives you a believable enterprise task management tool whose workflows, SLAs, approvals, and escalations are complex enough to show CQRS, mediator pipelines, rule evaluation, and permission checks, but still small enough to implement as a sample.  

If you’d like, I can next sketch the concrete bounded contexts and a minimal set of commands/events that would fit your .NET Aspire + mediator-style CQRS setup.