import { Fragment, useEffect, useState } from 'react'
import './App.css'
import { get } from './services/api';
import { SportingEvent } from './models/SportingEvent';

function App() {
  const [events, setEvents] = useState<SportingEvent[]>();

  useEffect(() => {
    const getEvents = async () => {
      try {
        const apiData = await get<SportingEvent[]>("/api/events");
        setEvents(apiData);
      } catch (e) {
        console.error(e);
      }
    }
    getEvents();
  }, [])

  return (
    <div>
      <p>Hello</p>
      {events?.map((e) => 
        <Fragment key={e.id}>{e.awayTeam.name}</Fragment>
      )}
    </div>
  )
}

export default App
