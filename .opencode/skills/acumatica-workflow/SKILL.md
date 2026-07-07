---
name: acumatica-workflow
description: Use when creating or modifying Acumatica workflow definitions using the 2021R1+ workflow framework. Covers state identifiers, flow states, actions, transitions, and conditions.
---

# Acumatica Workflow Framework Skill (2021R1+)

## Complete Workflow Template

```csharp
public class ATPTEFMEntityMaint_Workflow : PXGraphExtension<ATPTEFMEntityMaint>
{
    public override void Configure(PXScreenConfiguration config)
    {
        var context = config.GetScreenConfigurationContext<ATPTEFMEntityMaint, ATPTEFMEntity>();
        
        context.AddScreenConfigurationFor(screen =>
        {
            return screen
                .StateIdentifierIs<ATPTEFMEntity.status>()
                .AddDefaultFlow(flow => flow
                    .WithFlowStates(fss =>
                    {
                        fss.Add<ATPTEFMStatus.hold>(flowState =>
                        {
                            return flowState
                                .IsInitial()
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.release);
                                    actions.Add(g => g.hold);
                                });
                        });
                        
                        fss.Add<ATPTEFMStatus.balanced>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.release);
                                    actions.Add(g => g.hold);
                                });
                        });
                        
                        fss.Add<ATPTEFMStatus.released>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.reverse);
                                });
                        });
                    })
                    .WithTransitions(transitions =>
                    {
                        transitions.AddGroupFrom<ATPTEFMStatus.hold>(ts =>
                        {
                            ts.Add(t => t.To<ATPTEFMStatus.balanced>()
                                .IsTriggeredOn(g => g.release));
                        });
                        
                        transitions.AddGroupFrom<ATPTEFMStatus.balanced>(ts =>
                        {
                            ts.Add(t => t.To<ATPTEFMStatus.released>()
                                .IsTriggeredOn(g => g.release));
                        });
                        
                        transitions.AddGroupFrom<ATPTEFMStatus.released>(ts =>
                        {
                            ts.Add(t => t.To<ATPTEFMStatus.hold>()
                                .IsTriggeredOn(g => g.reverse));
                        });
                    }));
        });
    }
}
```

## Key Components

### State Identifier
```csharp
.StateIdentifierIs<ATPTEFMEntity.status>()
```

### Flow States
```csharp
fss.Add<ATPTEFMStatus.hold>(flowState =>
{
    return flowState
        .IsInitial()  // Starting state
        .WithActions(actions =>
        {
            actions.Add(g => g.release);  // Available actions
        });
});
```

### Transitions
```csharp
transitions.AddGroupFrom<ATPTEFMStatus.hold>(ts =>
{
    ts.Add(t => t.To<ATPTEFMStatus.released>()
        .IsTriggeredOn(g => g.release));  // Action triggers transition
});
```

### Conditions
```csharp
ts.Add(t => t.To<ATPTEFMStatus.released>()
    .IsTriggeredOn(g => g.release)
    .WithFieldAssignments(fa => fa.Add<ATPTEFMEntity.approved>(f => f.SetFromValue(true))));
```

## Workflow Action Registration
Actions used in workflows must be defined in the graph:
```csharp
public PXAction<ATPTEFMEntity> release;
[PXButton(CommitChanges = true)]
[PXUIField(DisplayName = ATPTEFMMessages.Release)]
protected virtual IEnumerable Release(PXAdapter adapter)
{
    // Implementation
    return adapter.Get();
}
```

## Common Patterns

### Simple Two-State Workflow
```csharp
context.AddScreenConfigurationFor(screen =>
{
    return screen
        .StateIdentifierIs<ATPTEFMEntity.status>()
        .AddDefaultFlow(flow => flow
            .WithFlowStates(fss =>
            {
                fss.Add<ATPTEFMStatus.hold>(flowState =>
                    flowState.IsInitial()
                        .WithActions(actions => actions.Add(g => g.release)));
                
                fss.Add<ATPTEFMStatus.released>(flowState =>
                    flowState.WithActions(actions => actions.Add(g => g.hold)));
            })
            .WithTransitions(transitions =>
            {
                transitions.AddGroupFrom<ATPTEFMStatus.hold>(ts =>
                    ts.Add(t => t.To<ATPTEFMStatus.released>()
                        .IsTriggeredOn(g => g.release)));
                
                transitions.AddGroupFrom<ATPTEFMStatus.released>(ts =>
                    ts.Add(t => t.To<ATPTEFMStatus.hold>()
                        .IsTriggeredOn(g => g.hold)));
            }));
});
```

### Workflow with Conditions
```csharp
fss.Add<ATPTEFMStatus.hold>(flowState =>
{
    return flowState
        .IsInitial()
        .WithActions(actions =>
        {
            actions.Add(g => g.release, a => a.IsDisabledWhen(g => g.Amount <= 0));
            actions.Add(g => g.delete);
        });
});
```
