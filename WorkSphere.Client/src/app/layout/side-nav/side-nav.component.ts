import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-side-nav',
  standalone: false,
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.css'
})
export class SideNavComponent implements OnInit {
  public activeItem?: string;


  sideNavItems = [
    {
      label: 'Dashboard',
      icon: 'dashboard',
      route: '',
      role: ['Admin', 'ProjectManager', 'Employee'], // Accessible to all
      children: null
    },
    {
      label: 'Manager Dashboard',
      icon: 'manage_accounts', // Material Icons
      route: '/manager-dashboard',
      role: ['ProjectManager'], // Only Project Managers can access
      children: null
    },
    {
      label: 'Project Management',
      icon: 'folder_open',
      route: '',
      role: ['Admin', 'ProjectManager'],
      children: [
        {
          label: 'Projects',
          icon: 'work',
          route: '/projects',
          role: ['Admin', 'ProjectManager'], // Only Admin and Project Managers can access
        },
        {
          label: 'Tasks',
          icon: 'check_circle',
          route: '/tasks',
          role: ['Admin', 'ProjectManager', 'Employee'], // Accessible to all
        },
        {
          label: 'Team Management',
          icon: 'group',
          route: '/team-management',
          role: ['Admin', 'ProjectManager'], // Only Admin and Project Managers can access
        },
        {
          label: 'Time Tracking',
          icon: 'access_time',
          route: '/time-tracking',
          role: ['Admin', 'ProjectManager', 'Employee'], // Accessible to all
        }
      ]
    },
    {
      label: 'Salary Management',
      icon: 'attach_money',
      route: '',
      role: ['Admin'],
      children: [
        {
          label: 'Salaries',
          icon: 'monetization_on',
          route: '/salaries',
          role: ['Admin'], // Only Admin can access
        },
        {
          label: 'Payroll',
          icon: 'payment',
          route: '/payroll',
          role: ['Admin'], // Only Admin can access
        }
      ]
    },
    {
      label: 'Communication',
      icon: 'chat',
      route: '',
      role: ['Admin', 'ProjectManager', 'Employee'],
      children: [
        {
          label: 'Chat',
          icon: 'message',
          route: '/chat',
          role: ['Admin', 'ProjectManager', 'Employee'], // Accessible to all
        },
        {
          label: 'Messages',
          icon: 'inbox',
          route: '/messages',
          role: ['Admin', 'ProjectManager', 'Employee'], // Accessible to all
        },
        {
          label: 'Notifications',
          icon: 'notifications',
          route: '/notifications',
          role: ['Admin', 'ProjectManager', 'Employee'], // Accessible to all
        },
        {
          label: 'Announcements',
          icon: 'announcement',
          route: '/announcements',
          role: ['Admin', 'ProjectManager'], // Admin and Project Managers can post announcements
        }
      ]
    },
    {
      label: 'Analytics & Reports',
      icon: 'assessment',
      route: '',
      role: ['Admin', 'ProjectManager'],
      children: [
        {
          label: 'Reports',
          icon: 'bar_chart',
          route: '/reports',
          role: ['Admin', 'ProjectManager'], // Only Admin and Project Manager can access
        },
        {
          label: 'Analytics',
          icon: 'show_chart',
          route: '/analytics',
          role: ['Admin', 'ProjectManager'], // Only Admin and Project Manager can access
        },
      ]
    },
    {
      label: 'User Management',
      icon: 'supervised_user_circle',
      route: '',
      role: ['Admin'],
      children: [
        {
          label: 'Users',
          icon: 'person',
          route: '/users',
          role: ['Admin'], // Only Admin can manage users
        },
        {
          label: 'Roles & Permissions',
          icon: 'lock',
          route: '/roles',
          role: ['Admin'], // Only Admin can manage roles and permissions
        }
      ]
    },
    {
      label: 'Settings',
      icon: 'settings',
      route: '/settings',
      role: ['Admin', 'ProjectManager'], // Only Admin and Project Manager can access
      children: null
    },
    {
      label: 'Admin Tools',
      icon: 'build',
      route: '',
      role: ['Admin'],
      children: [
        {
          label: 'System Logs',
          icon: 'receipt_long',
          route: '/system-logs',
          role: ['Admin'], // Only Admin can view system logs
        },
        {
          label: 'Audit Trails',
          icon: 'history',
          route: '/audit-trails',
          role: ['Admin'], // Only Admin can view audit trails
        },
        {
          label: 'Backup & Restore',
          icon: 'backup',
          route: '/backup-restore',
          role: ['Admin'], // Only Admin can manage backup and restore
        }
      ]
    }
  ];

  constructor(private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.route.url.subscribe((url) => {
      this.activeItem = url[0].path;
    });
  }

  public setActiveItem(item: string): void {
    this.activeItem = item;
  }


  closeSideNav() {
    const sideNav = document.getElementById('side-nav');
    //only set the width to 0 if the screen is less than 600px
    if (sideNav && window.innerWidth < 768) {
      sideNav.style.width = '0';
    }
  }
}
