"use client"
import React from 'react'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { useAuthStore } from '@/store/auth';
import Loading from '@/components/ui/loading';

export default function UserProfilePage() {
	const user = useAuthStore((state) => state.user);

	if (!user) {
		return (
			<Loading />
		)
	}

  return (
    <div className='container mx-auto p-4'>
      <div className='card'>
        <div >
          <h2 className='text-2xl font-bold mb-4 text-gray-900'>User Profile</h2>
        </div>
        <div>
          <div className='flex items-center space-x-4'>
            <Avatar>
              <AvatarImage src={user.avatarUrl} alt="User Avatar" />
              <AvatarFallback>UN</AvatarFallback>
            </Avatar>
            <div>
              <h3 className='text-xl font-semibold'>{user.name}</h3>
              <p className='text-gray-500'>{user.email}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}