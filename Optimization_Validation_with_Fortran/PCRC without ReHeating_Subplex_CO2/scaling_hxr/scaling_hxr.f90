!-----------------------------------------------------------------------------------------------------------------------------------
!
! This is free and unencumbered software released into the public domain.
! 
! Anyone is free to copy, modify, publish, use, compile, sell, or distribute this software, either in source code form or as a
! compiled binary, for any purpose, commercial or non-commercial, and by any means.
! 
! THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
! MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM,
! DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
! SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
!
!-----------------------------------------------------------------------------------------------------------------------------------
!
! This file contains the the module 'heat_exchangers', which defines heat exchanger pressure drop and conductance scaling functions.
!
! Notes:
!   1) Pressure drops are scaled with mass flow rate according to the Darcy friction factor and Blasius correlation.
!   2) Conductance values are scaled with mass flow rate according to the Dittus-Boelter heat transfer correlation.
!
! Author: John Dyreby, Solar Energy Laboratory, University of Wisconsin-Madison <jjdyreby@uwalumni.com>
!
! Last Modified: July 12, 2014
!
!-----------------------------------------------------------------------------------------------------------------------------------

module heat_exchangers

use core
implicit none
private
public :: hxr_pressure_drops, hxr_conductance

contains

function hxr_pressure_drops(hxr, m_dots)
    ! Return an array of the scaled pressure drops (in kPa) for the two streams of the heat exchanger defined by 'hxr'.
    !
    ! Inputs:
    !   hxr -- a HeatExchanger type with design-point values set
    !   m_dots -- mass flow rates of the two streams (kg/s) [1: cold, 2: hot]
    !
    type(HeatExchanger), intent(in) :: hxr
    real(dp), dimension(2), intent(in) :: m_dots
    real(dp), dimension(2) :: hxr_pressure_drops
    hxr_pressure_drops = hxr%DP_design * (m_dots / hxr%m_dot_design)**1.75_dp  ! operates on both streams simultaneously
end function hxr_pressure_drops

real(dp) function hxr_conductance(hxr, m_dots)
    ! Return the scaled conductance (in kW/K) of the heat exchanger defined by 'hxr'.
    !
    ! Inputs:
    !   hxr -- a HeatExchanger type with design-point values set
    !   m_dots -- mass flow rates of the two streams (kg/s) [1: cold, 2: hot]
    !
    type(HeatExchanger), intent(in) :: hxr
    real(dp), dimension(2), intent(in) :: m_dots
    real(dp) :: m_dot_ratio
    m_dot_ratio = (m_dots(1) / hxr%m_dot_design(1) + m_dots(2) / hxr%m_dot_design(2)) * 0.5_dp  ! average the two streams
    hxr_conductance = hxr%UA_design * m_dot_ratio**0.8_dp
end function hxr_conductance

end module heat_exchangers
