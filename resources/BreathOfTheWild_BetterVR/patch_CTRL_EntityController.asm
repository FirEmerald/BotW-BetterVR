[BetterVR_EntityController_V208]
moduleMatches = 0x6267BFD0

.origin = codecave

; hooks entity transform and velocity updates
0x0348970C = ba import.coreinit.hook_SetRigidBodyVelocity
0x03486CD4 = ba import.coreinit.hook_SetRigidBodyTransform
0x03486D80 = ba import.coreinit.hook_SetRigidBodyPosition
0x03489A80 = ba import.coreinit.hook_SetRigidBodyPositionAndRotation
0x0348AB20 = ba import.coreinit.hook_SetRigidBodyRotation


; hooks arrow shooting parameters (and other dynamically loaded bools/values inside AI Actions)
0x030EC840 = ba import.coreinit.hook_LoadDynamicVec3
0x030EC8C0 = bla import.coreinit.hook_LoadDynamicBool

0x1011A7A0 = str_IsShootByPlayer:
0x1011A7F4 = str_TargetPos:
0x030EC860 = ksys__act__ai__InlineParamPack__addBool:
0x030EC7DC = ksys__act__ai__InlineParamPack__addVec3:

; swap TargetPos and IsShootByPlayer to be able to mutate the TargetPos value AFTER we know that the arrow is shot by the player

; .origin = 0x02705AFC
; 
; lis r6, str_IsShootByPlayer@ha
; addi r6, r6, str_IsShootByPlayer@l
; lwz r5, 0x0E8(r29)
; stw r27, 0x014(r1)
; stw r6, 0x010(r1)
; lwz r8, 0x4EC(r5)
; mtctr r8
; mr r3, r29
; bctrl
; mr r4, r3
; li r6, -1
; addi r5, r1, 0x010
; addi r3, r1, 0x158
; bl ksys__act__ai__InlineParamPack__addBool
; 
; lis r4, str_TargetPos@ha
; addi r4, r4, str_TargetPos@l
; li r6, -1
; addi r5, r1, 0x010
; stw r4, 0x010(r1)
; addi r4, r1, 0x88
; addi r3, r1, 0x158
; stw r27, 0x14(r1)
; bl ksys__act__ai__InlineParamPack__addVec3
; 
; .origin = codecave