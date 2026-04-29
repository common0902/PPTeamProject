using System;
using UnityEngine;

public class StateTransition<T>
{
    // Transition Command가 없음을 나타냄
    public const int kNullCommand = int.MinValue;

    // Transition을 위한 조건 함수, 인자는 현재 State, 결과값은 전이 가능 여부(bool)
    private Func<State<T>, bool> transitionCondition;

    // 현재 State에서 다시 현재 State로 전이가 가능하지에 대한 여부
    public bool CanTrainsitionToSelf { get; private set; }
    // 현재 State
    public State<T> FromState { get; private set; }
    // 전이할 State
    public State<T> ToState { get; private set; }
    // 전이 명령어
    public int TransitionCommand { get; private set; }
    // 전이 가능 여부(Condition 조건 만족 여부), 트랜지션 조건이 없거나 트랜지션조건이 true일경우
    public bool IsTransferable => transitionCondition == null || transitionCondition.Invoke(FromState);

    public StateTransition(State<T> fromState,
        State<T> toState,
        int transitionCommand,
        Func<State<T>, bool> transitionCondition,
        bool canTrainsitionToSelf)
    {
        Debug.Assert(transitionCommand != kNullCommand || transitionCondition != null,
            "StateTransition - TransitionCommand와 TransitionCondition은 둘 다 null이 될 수 없습니다.");

        FromState = fromState;
        ToState = toState;
        TransitionCommand = transitionCommand;
        this.transitionCondition = transitionCondition;
        CanTrainsitionToSelf = canTrainsitionToSelf;
    }
}
